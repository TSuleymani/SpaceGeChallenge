using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGeApp.Core.Common.Models
{
   public class WatchlistItemsResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int MovieId { get; set; }
        public bool Watched { get; set; }
    }
}
