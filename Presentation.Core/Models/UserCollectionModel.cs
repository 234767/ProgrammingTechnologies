using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Abstractions;

namespace Presentation.Core.Models
{
    public class UserCollectionModel
    {
        private ILibraryService _library;

        public UserCollectionModel( ILibraryService library )
        {
            _library = library;
        }

        public async Task<IList<IUserModel>> Search( string? name )
        {
            return (await _library.SearchUsers( name )).ToList();
        }

        public UserModel GetNewUser()
        {
            return new UserModel( _library );
        }
    }
}
