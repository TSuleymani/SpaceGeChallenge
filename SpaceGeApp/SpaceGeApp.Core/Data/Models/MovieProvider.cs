using System;
using System.Collections.Generic;
using System.Text;
using SpaceGeApp.Core.Models;

namespace SpaceGeApp.Core.Data.Models
{
   public class MovieProvider : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
