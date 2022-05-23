using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    public class BookDto : IBook
    {
        public string Id { get; init; }

        [ForeignKey(nameof(BookInfo))]
        public string BookInfoId { get; set; }

        public virtual IBookInfo BookInfo { get; set; }
    }
}
