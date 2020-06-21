using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    /// <summary>
    /// Book object for Creation
    /// </summary>
    public class BookForCreation
    {
        /// <summary>
        /// Author id
        /// </summary>
        public string AuthorId { get; set; }

        /// <summary>
        /// Title of Book
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        /// <summary>
        /// Description of Book
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
    }
}
