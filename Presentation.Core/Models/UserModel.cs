using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Abstractions;

namespace Presentation.Core.Models
{
    public class UserModel : IUserModel
    {
        public ILibraryService Library { get; }

        public UserModel( ILibraryService library )
        {
            Library = library;
        }

        public string Id { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string Surname { get; set; } = string.Empty;

        public async Task Crete()
        {
            await Library.AddUser(this);
        }

        public async Task Save()
        {
            await Library.SaveUser(this);
        }

        public async Task Delete()
        {
            await Library.RemoveUser(this.Id);
        }
    }
}
