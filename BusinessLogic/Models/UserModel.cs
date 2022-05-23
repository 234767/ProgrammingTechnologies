using BusinessLogic.Abstractions;

namespace BusinessLogic.Models;

internal class UserModel : IUserModel
{
    public ILibraryService Library { get; }

    public UserModel( ILibraryService library, string id, string firstName, string surname )
    {
        Library = library;
        Id = id;
        FirstName = firstName;
        Surname = surname;
    }

    public string Id { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }

    public async Task Create()
    {
        await Library.AddUser( this );
    }

    public async Task Save()
    {
        await Library.SaveUser( this );
    }

    public async Task Delete()
    {
        await Library.RemoveUser( this.Id );
    }
}