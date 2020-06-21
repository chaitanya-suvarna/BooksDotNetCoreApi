using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Books.Contexts;
using Books.Entities;
using Books.ExternalModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Books.Services
{
    public class BooksRepository : IBooksRepository, IDisposable
    {
        private BooksContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BooksRepository> _logger;
        private CancellationTokenSource _cancellationTokenSource;

        public BooksRepository(BooksContext context, IHttpClientFactory httpClientFactory,
            ILogger<BooksRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Book> GetBookAsync(Guid id)
        {
            return await _context.Books.Include(b => b.Author).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await _context.Books.Include(b => b.Author).ToListAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }

                if(_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }
            }
        }

        public void AddBook(Book bookToAdd)
        {
            if(bookToAdd == null)
            {
                throw new ArgumentNullException(nameof(bookToAdd));
            }

            _context.Add(bookToAdd);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(IEnumerable<Guid> bookIds)
        {
            return await _context.Books.Where(b => bookIds.Contains(b.Id))
                            .Include(b => b.Author).ToListAsync();
        }

        public async Task<BookCover> GetBookCoverAsync(string coverId)
        {
            var httpClient = _httpClientFactory.CreateClient();

            var response = await httpClient
                .GetAsync($"http://localhost:59956/api/bookcovers/{coverId}");

            if(response.IsSuccessStatusCode)
            {
                return JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true
                    });
            }
            else
            {
                return null;
            }
        }

        public async Task<BookCover> DownloadBookCoverAsync(
            HttpClient httpClient, string bookCoverUrl, CancellationToken cancellationToken)
        {
            var response = await httpClient.GetAsync(bookCoverUrl,cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                var bookCover = JsonSerializer.Deserialize<BookCover>(
                    await response.Content.ReadAsStringAsync(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                return bookCover;
            }

            _cancellationTokenSource.Cancel();
            return null;
        }

        public async Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var bookCovers = new List<BookCover>();
            _cancellationTokenSource = new CancellationTokenSource();

            var bookCoverUrls = new[]
            {
                $"http://localhost:59956/api/bookcovers/{bookId}-dummycover1",
                $"http://localhost:59956/api/bookcovers/{bookId}-dummycover2?returnFault=true",
                $"http://localhost:59956/api/bookcovers/{bookId}-dummycover3",
                $"http://localhost:59956/api/bookcovers/{bookId}-dummycover4",
                $"http://localhost:59956/api/bookcovers/{bookId}-dummycover5"
            };

            var downloadBookCoverTasksQuery = bookCoverUrls.
                Select(bookCoverUrl => DownloadBookCoverAsync(httpClient, bookCoverUrl, 
                _cancellationTokenSource.Token));

            var downloadBookCoverTasks = downloadBookCoverTasksQuery.ToList();

            try
            {
                return await Task.WhenAll(downloadBookCoverTasks);
            }
            catch (OperationCanceledException operationCanceledException)
            {
                _logger.LogInformation($"{operationCanceledException.Message}");
                foreach (var task in downloadBookCoverTasks)
                {
                    _logger.LogInformation($"Task {task.Id} has status {task.Status}");
                }

                return new List<BookCover>();
            }
            catch (Exception exception)
            {
                _logger.LogError($"{exception.Message}");
                throw;
            }
            /*
            foreach (var bookCoverUrl in bookCoverUrls)
            {
                var response = await httpClient.GetAsync(bookCoverUrl);

                if (response.IsSuccessStatusCode)
                {
                    bookCovers.Add(JsonSerializer.Deserialize<BookCover>(
                        await response.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        }
                        ));
                }
            }

            return bookCovers; 
            */
        }
    }
}
