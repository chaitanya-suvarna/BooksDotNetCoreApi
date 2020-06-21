using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.ExternalModels
{
    /// <summary>
    /// A BookCover with Name and Content
    /// </summary>
    public class BookCover
    {
        /// <summary>
        /// The name of the BookCover
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The book cover content in byte-array format
        /// </summary>
        public byte[] Content { get; set; }
    }
}
