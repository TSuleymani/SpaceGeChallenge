using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceGeApp.Core.Data;

using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SpaceGeApp.Core.Common;
using SpaceGeApp.Core.Common.Models;

namespace SpaceGeApp.Core.Services
{
   public class SchedulerService
    {
        private readonly MovieDbContext _movieDb;
        private readonly IEmailSender _emailSender;
        private readonly IMovieProvider _provider;
        public SchedulerService(MovieDbContext movieDb, IEmailSender emailSender, IMovieProvider provider)
        {
            this._movieDb = movieDb ?? throw new ArgumentNullException(nameof(MovieDbContext));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(IEmailSender));
            this._provider = provider ?? throw new ArgumentNullException(nameof(IMovieProvider));
        }
        private async Task<IEnumerable<FilmDetailsResponseModel>> GetUnwatchedFilmDetailsPerUserAsync()
        {
            var users = new List<FilmDetailsResponseModel>();

           var userMovies = await _movieDb.WatchList
                                       .Include("User")
                                        .Where(x => !x.Watched)
                                        .Select(x => new { x.UserId, x.User.Email, x.Movie.MovieId,x.Movie.Id })
                                        .ToListAsync();

           var totalUsers = userMovies.GroupBy(n => n.Email)
                                        .Select(n => new
                                        {
                                            n.Key,
                                            Count = n.Count()
                                        })
                                        .ToList();



            foreach (var user in totalUsers)
            {
                var movieDescriptionBuilder = new StringBuilder();
               var currentUserMovies = userMovies.Where(x => x.Email == user.Key).Select(t => t.MovieId).ToList();
                foreach (var item in currentUserMovies)
                {
                  var movieDetailedDescription = await _provider.GetMovieDetailedDescriptionAsync(new MovieDetailsRequest
                  {
                       Language = "en",
                        MovieId = item
                  });
                    movieDescriptionBuilder.Append($"{movieDetailedDescription.Result.Title} {movieDetailedDescription.Result.Year}");
                }
                users.Add(new FilmDetailsResponseModel
                {
                     Email = user.Key,
                     FormattedText = movieDescriptionBuilder.ToString()
                });
            }
            return users;
        }

        public async Task ScheduleEmailAsync()
        {
            var filmDetailsInfo = await GetUnwatchedFilmDetailsPerUserAsync();

            foreach (var filmDetailInfo in filmDetailsInfo)
            {
               await _emailSender.SendEmailAsync(filmDetailInfo.Email, "Notification", filmDetailInfo.FormattedText);
            }
        }
    }
}
