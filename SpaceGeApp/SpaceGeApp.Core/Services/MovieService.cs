using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SpaceGeApp.Core.Common;
using SpaceGeApp.Core.Common.Abstract;
using SpaceGeApp.Core.Common.Models;
using SpaceGeApp.Core.Data;
using SpaceGeApp.Core.Exceptions;
using SpaceGeApp.Core.Extensions;
using SpaceGeApp.Core.Validators;

namespace SpaceGeApp.Core.Services
{
   public class MovieService : IMovieService
    {
        private readonly MovieServiceOptions _options;
        private readonly IMovieProvider _provider;
        private readonly MovieDbContext _movieDb;
        private int _providerId;
        public MovieService(MovieServiceOptions options
                           , IMovieProvider provider
                           , MovieDbContext movieDbContext)
        {
            this._options = options ?? throw new ArgumentNullException(nameof(MovieServiceOptions));
            this._provider = provider ?? throw new ArgumentNullException(nameof(IMovieProvider));
            this._movieDb = movieDbContext ?? throw new ArgumentNullException(nameof(MovieDbContext));
            _providerId = this._movieDb.MovieProviders.FirstOrDefault(x => x.Name == "IMDB").Id;
        }

        public async Task<SearchResponseModel> GetMoviesByNameAsync(SearchRequestModel searchRequest)
        {
            var validator = new SearchRequestValidator();

            var validationResult = validator.Validate(searchRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToPairedErrors());
            }
            try
            {
                var response = await _provider.GetMovieByNameAsync(new SearchRequest
                {
                    Expression = searchRequest.Expression,
                    Language = "en"
                });

                if (response.IsSuccess)
                {
                    return new SearchResponseModel()
                    {
                         Results = response.Result.Results,
                          Expression = response.Result.Expression,
                           SearchType = response.Result.SearchType
                    };
                }
                else
                {
                    throw new ServiceException(ExceptionType.External, response.Errors);
                }
            }
            catch (ServiceException ex)
            {
               //some logging here
                throw ex;
            }
            catch (ProviderException ex)
            {
                //some logging here
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                //some logging here
                throw ex;
            }
        }

        public async Task<UserWatchlistResponseModel> AddMovieToWatchListAsync(UserWatchlistModel userWatchlist)
        {
            var validator = new UserWatchListValidator();

            var validationResult = validator.Validate(userWatchlist);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToPairedErrors());
            }
            try
            {
                //check if user exists
              var isUserExists = await _movieDb.Users.FirstOrDefaultAsync(x => x.Id == userWatchlist.UserId);

                if(isUserExists == null)
                {
                    IEnumerable<string> errors = new string[] { $"userId = {userWatchlist.UserId} doesn't exists" };
                    throw new ServiceException(ExceptionType.Internal, errors);
                }

                //check if movie already added
                var isAlreadyAddedToMovies = await _movieDb.Movies
                                  .FirstOrDefaultAsync(x => x.MovieId == userWatchlist.MovieId);
;
                if(isAlreadyAddedToMovies != null)
                {
                    //check if movie + user combination already exists in watchlist
                    var isAlreadyAddedToWatchList = await _movieDb.WatchList
                              .FirstOrDefaultAsync(x => x.UserId == userWatchlist.UserId && x.MovieId == isAlreadyAddedToMovies.Id);

                    if(isAlreadyAddedToWatchList != null)
                    {
                        IEnumerable<string> errors = new string[] { $"Movie with id = {userWatchlist.MovieId} already exists for userId = {userWatchlist.UserId}" };
                        throw new ServiceException(ExceptionType.Internal, errors);
                    }
                    else
                    {
                       await _movieDb.WatchList.AddAsync(new Data.Models.WatchList
                        {
                            MovieId = isAlreadyAddedToMovies.Id,
                            UserId = userWatchlist.UserId,
                            Watched = false
                        });

                        await _movieDb.SaveChangesAsync();

                        return new UserWatchlistResponseModel
                        {
                            Message = "Add to watchlist",
                            SuccessfullyAdded = true
                        };
                    }

                }
                else
                {
                    //get movie details from the service
                    var movieDetails = await _provider.GetMovieByIdAsync(new ReviewRequest
                    {
                        Id = userWatchlist.MovieId,
                        Language = userWatchlist.Language
                    });
                    if (movieDetails.IsSuccess)
                    {
                        var movieInfo = new Data.Models.Movie
                        {
                            MovieId = movieDetails.Result.IMDbId,
                            Name = movieDetails.Result.Title,
                             MovieProviderId = _providerId
                        };
                        //add movie details to DB
                        await _movieDb.Movies.AddAsync(movieInfo);
                        await _movieDb.SaveChangesAsync();

                        if(movieInfo.Id > 0)
                        {
                            await _movieDb.WatchList.AddAsync(new Data.Models.WatchList
                            {
                                MovieId = movieInfo.Id,
                                UserId = userWatchlist.UserId,
                                Watched = false
                            });

                            await _movieDb.SaveChangesAsync();
                            return new UserWatchlistResponseModel
                            {
                                Message = "Add to watchlist",
                                SuccessfullyAdded = true
                            };
                        }

                    }
                    else
                    {
                        throw new ServiceException(ExceptionType.Internal, movieDetails.Errors);
                    }
                }
                return new UserWatchlistResponseModel
                {
                    Message = "Add to watchlist",
                    SuccessfullyAdded = true
                };
            }
            catch (ServiceException ex)
            {
                //some logging here
                throw ex;
            }
            catch (ProviderException ex)
            {
                //some logging here
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                //some logging here
                throw ex;
            }
        }

        public async Task<IEnumerable<WatchlistItemsResponseModel>> GetWatchListByUserAsync(WatchlistItemsRequestModel watchlistItemsRequest)
        {
            var validator = new WatchlistItemsValidator();

            var validationResult = validator.Validate(watchlistItemsRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToPairedErrors());
            }

            try
            {
                //check if user exists
                var isUserExists = await _movieDb.Users.FirstOrDefaultAsync(x => x.Id == watchlistItemsRequest.UserId);

                if (isUserExists == null)
                {
                    IEnumerable<string> errors = new string[] { $"userId = {watchlistItemsRequest.UserId} doesn't exists" };
                    throw new ServiceException(ExceptionType.Internal, errors);
                }

              var userInWatchList =  _movieDb.WatchList.FirstOrDefaultAsync(x => x.UserId == watchlistItemsRequest.UserId);
                if(userInWatchList == null)
                {
                    IEnumerable<string> errors = new string[] { $"userId = {watchlistItemsRequest.UserId} doesn't have any watchlist item" };
                    throw new ServiceException(ExceptionType.Internal, errors);
                }

              var watchListResponse = await _movieDb.WatchList.Include("Movie").Where(x => x.UserId == watchlistItemsRequest.UserId)
                    .Select(x => new WatchlistItemsResponseModel
                    {
                        Id = x.Id,
                        MovieId = x.MovieId,
                        Title = x.Movie.Name,
                        Watched = x.Watched
                    }).ToListAsync();

                return watchListResponse;

            }
            catch (ServiceException ex)
            {
                //some logging here
                throw ex;
            }
            catch (ProviderException ex)
            {
                //some logging here
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                //some logging here
                throw ex;
            }
        }

        public async Task<bool> MarkMovieAsWatchedAsync(MarkWatchlistItemsRequestModel watchlistItemsRequest)
        {
            var validator = new MarkWatchListItemsValidator();

            var validationResult = validator.Validate(watchlistItemsRequest);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.ToPairedErrors());
            }

            try
            {
                //check if user exists
                var isUserExists = await _movieDb.Users.FirstOrDefaultAsync(x => x.Id == watchlistItemsRequest.UserId);

                if (isUserExists == null)
                {
                    IEnumerable<string> errors = new string[] { $"userId = {watchlistItemsRequest.UserId} doesn't exists" };
                    throw new ServiceException(ExceptionType.Internal, errors);
                }

                //check if movie already added
                var isAlreadyAddedToMovies = await _movieDb.Movies
                                  .FirstOrDefaultAsync(x => x.MovieId == watchlistItemsRequest.MovieId);

                if(isAlreadyAddedToMovies == null)
                {
                    IEnumerable<string> errors = new string[] { $"movieId = {watchlistItemsRequest.MovieId} doesn't exists" };
                    throw new ServiceException(ExceptionType.Internal, errors);
                }
                //check if movie + user combination already exists in watchlist
                var isAlreadyAddedToWatchList = await _movieDb.WatchList
                          .FirstOrDefaultAsync(x => x.UserId == watchlistItemsRequest.UserId && x.MovieId == isAlreadyAddedToMovies.Id);

                if (isAlreadyAddedToWatchList == null)
                {
                    IEnumerable<string> errors = new string[] { $"Movie with id = {isAlreadyAddedToWatchList.MovieId} not binded  for userId = {watchlistItemsRequest.UserId}" };
                    throw new ServiceException(ExceptionType.Internal, errors);
                }
                else
                {
                   var watchListElement = await _movieDb.WatchList.FirstOrDefaultAsync(x => x.MovieId == isAlreadyAddedToMovies.Id && x.UserId == watchlistItemsRequest.UserId);
                    watchListElement.Watched = true;
                    await _movieDb.SaveChangesAsync();

                    return true;
                }

            }
            catch (ServiceException ex)
            {
                //some logging here
                throw ex;
            }
            catch (ProviderException ex)
            {
                //some logging here
                throw new Exception(ex.Message);
            }
            catch (Exception ex)
            {
                //some logging here
                throw ex;
            }
        }


    }
}
