using DataAccess.API.DTO;

namespace DataAccess.Database.Dto
{
    public class UserDto : IUser
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
    }
}
