using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Abstractions;
using BusinessLogic.Abstractions.Models;

namespace Presentation.Core.Models
{
    public class BookCollectionModel
    {
        private ILibraryService _library;

        public BookCollectionModel(ILibraryService library)
        {
            _library = library;
        }

        public async Task<IList<IBookModel>> Search(string property, string? name)
        {
            return (await _library.SearchBooks(property, name)).ToList();
        }

        public BookModel GetNewBook()
        {
            return new BookModel(_library);
        }
    }
}
