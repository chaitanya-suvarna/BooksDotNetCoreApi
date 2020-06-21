using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class BookForCreation
    {
        public string AuthorId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
