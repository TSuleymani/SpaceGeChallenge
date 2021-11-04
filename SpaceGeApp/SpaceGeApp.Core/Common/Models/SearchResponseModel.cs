using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGeApp.Core.Common.Models
{
   public class SearchResponseModel
    {
        public string SearchType { get; set; }
        public string Expression { get; set; }

        public List<SearchResponse> Results { get; set; }

        public string ErrorMessage { get; set; }
    }
}
