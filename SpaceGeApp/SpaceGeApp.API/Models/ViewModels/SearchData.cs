using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceGeApp.API.Models
{
    public class SearchData :BaseSearchData
    {
        public string? Language { get; set; }
        public string Expression { get; set; }
    }
}
