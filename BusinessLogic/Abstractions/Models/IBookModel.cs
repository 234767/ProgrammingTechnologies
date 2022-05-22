using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstractions.Models
{
    public interface IBookModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateOnly? DatePublished { get; set; }

        public bool IsAvailable { get; }

        public Task DeleteAsync();
    }
}
