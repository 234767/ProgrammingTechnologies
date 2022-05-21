using System;
using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    public class BookInfoDto : IBookInfo
    {
        public string Id { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public DateTime? DatePublished { get; set; } = null!;
    }
}
