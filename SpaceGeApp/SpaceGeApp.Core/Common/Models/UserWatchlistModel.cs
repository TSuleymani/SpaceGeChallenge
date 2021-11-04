using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceGeApp.Core.Common.Models
{
   public class UserWatchlistModel
    {
        public string Language { get; set; } = "en";
        public int UserId { get; set; }
        public string MovieId { get; set; }
        public bool Watched { get; set; } = false;
    }
}
