namespace BusinessLogic.Abstractions;
public interface IUserModel
{
    public ILibraryService Library { get; }
    string Id { get; set; }
    string FirstName { get; set; }
    string Surname { get; set; }
    Task Crete();
    Task Save();
    Task Delete();
}