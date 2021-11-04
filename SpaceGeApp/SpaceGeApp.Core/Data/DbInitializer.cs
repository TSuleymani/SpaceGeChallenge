using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceGeApp.Core.Data.Models;
using SpaceGeApp.Core.Models;

namespace SpaceGeApp.Core.Data
{
    public static class DbInitializer
    {
        public static void Initialize(MovieDbContext context)
        {
            // Lets check for any users.
            if (context.Users.Any())
            {
                return;   // seed already done
            }

            var users = new User[]
            {
                new User {
                    FirstName = "Simon",
                    LastName = "Panda",
                    Email = "simon.panda@gmail.com"
                },
                new User {
                    FirstName = "Joseph",
                    LastName = "Albahari",
                    Email = "joseph.albahari@gmail.com"
                },
            };

            var movieProvider = new MovieProvider()
            {
              Name = "IMDB"
            };

            context.MovieProviders.Add(movieProvider);
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}
