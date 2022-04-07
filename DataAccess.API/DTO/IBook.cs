namespace DataAccess.API.DTO;

public interface IBook
{
    public string Id { get; }
    public IBookInfo BookInfo { get; }
}