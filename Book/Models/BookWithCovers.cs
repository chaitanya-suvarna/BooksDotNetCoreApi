using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    /// <summary>
    ///  A Book object with all it's covers
    /// </summary>
    public class BookWithCovers : Book
    {
        /// <summary>
        /// A collection of covers for the book
        /// </summary>
        public IEnumerable<BookCover> BookCovers { get; set; } = new List<BookCover>();
    }
}
