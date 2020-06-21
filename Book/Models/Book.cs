using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    /// <summary>
    /// Book object
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Id for Book
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// Book author name
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Author { get; set; }
        /// <summary>
        /// Title of the book
        /// </summary>
        [Required]
        [MaxLength(150)]
        public string Title { get; set; }
        /// <summary>
        /// Description for the book
        /// </summary>
        [MaxLength(2500)]
        public string Description { get; set; }
        
    }
}
