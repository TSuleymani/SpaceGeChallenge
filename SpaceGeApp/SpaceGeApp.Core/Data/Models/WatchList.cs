using System;
using System.Collections.Generic;
using System.Text;
using SpaceGeApp.Core.Models;

namespace SpaceGeApp.Core.Data.Models
{
   public class WatchList : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public bool Watched { get; set; }

    }
}
