using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    internal class BookDto : IBook
    {
        public string Id { get; set; } = null!;

        public IBookInfo BookInfo { get; set; } = null!;
    }
}
