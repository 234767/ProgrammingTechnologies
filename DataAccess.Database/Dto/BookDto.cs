using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    public class BookDto : IBook
    {
        public string Id { get; init; }

        public IBookInfo BookInfo { get; set; }
    }
}
