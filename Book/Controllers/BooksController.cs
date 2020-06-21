using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Books.Filters;
using Books.Models;
using Books.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Books.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository;
        private readonly IMapper _mapper;

        public BooksController(IBooksRepository booksRepository,IMapper mapper)
        {
            _booksRepository = booksRepository ?? throw new ArgumentNullException(nameof(booksRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [BooksResultFilter()]
        public async Task<IActionResult> GetBooks()
        {
            var bookEntities = await _booksRepository.GetBooksAsync();
            return Ok(bookEntities);
        }

        [HttpGet]
        [Route("{id}", Name = "GetBook")]
        [BookWithCoverResultFilter]
        public async Task<IActionResult> GetBook(Guid id)
        {
            var bookEntity = await _booksRepository.GetBookAsync(id);
            if(bookEntity == null)
            {
                return NotFound();
            }

            var bookCovers = await _booksRepository.GetBookCoversAsync(id);

            return Ok((bookEntity,bookCovers));
        }

        [HttpPost]
        [BookResultFilter]
        public async Task<IActionResult> CreateBook(BookForCreation bookForCreation)
        {
            var bookEntity = _mapper.Map<Entities.Book>(bookForCreation);
            _booksRepository.AddBook(bookEntity);
            await _booksRepository.SaveChangesAsync();

            await _booksRepository.GetBookAsync(bookEntity.Id);

            return CreatedAtRoute("GetBook", new { id = bookEntity.Id }, bookEntity);
        }
    }
}