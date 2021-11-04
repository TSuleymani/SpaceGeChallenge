using System;
using System.Collections.Generic;
using System.Text;
using SpaceGeApp.Core.Models;

namespace SpaceGeApp.Core.Data.Models
{
    //for storing movie details
   public class Movie : IEntity
    {
        public int Id { get; set; }
        //user can have multiple source provider movies
        public string MovieId { get; set; }
        public string Name { get; set; }
        public int MovieProviderId { get; set; }
        public MovieProvider MovieProvider { get; set; }

    }
}
