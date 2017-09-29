using System;

namespace CitationBook.Domain.Entities
{
    public class Citation
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Author { get; set; }
        public DateTime CreateDate { get; set; }
        public int CategoryId { get; set; }
    }
}