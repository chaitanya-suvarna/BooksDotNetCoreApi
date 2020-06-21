namespace Books.Models
{
    /// <summary>
    /// Book Cover
    /// </summary>
    public class BookCover
    {
        /// <summary>
        /// Name of the Book Cover
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Content of the book Cover in byte-array format
        /// </summary>
        public byte[] Content { get; set; }
    }
}
