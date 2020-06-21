using Books.ExternalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Services
{
    public interface IBooksRepository
    {
        Task<IEnumerable<Entities.Book>> GetBooksAsync();

        Task<Entities.Book> GetBookAsync(Guid id);

        void AddBook(Entities.Book bookToAdd);

        Task<bool> SaveChangesAsync();

        Task<IEnumerable<Entities.Book>> GetBooksAsync(IEnumerable<Guid> bookIds);

        Task<BookCover> GetBookCoverAsync(string coverId);

        Task<IEnumerable<BookCover>> GetBookCoversAsync(Guid bookId);
    }
}
