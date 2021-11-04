using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpaceGeApp.API.Models;
using SpaceGeApp.Core.Common.Abstract;
using SpaceGeApp.Core.Common.Models;
using SpaceGeApp.Core.Exceptions;

namespace SpaceGeApp.API.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService paymentService)
        {
            this._movieService = paymentService;
        }

        [HttpGet]
        [Route("{lang}/API/Search/{expression}")]
        [ProducesResponseType(typeof(IEnumerable<SearchResult>), 200)]
        public async Task<ActionResult> GetFilmsByName( string expression, string lang = "en")
        {
            try
            {
              var movieResponse = await _movieService.GetMoviesByNameAsync(new SearchRequestModel
               {
                    Expression = expression,
                    Language = lang
               });
                return Ok(new ApiResponseModel<SearchResponseModel>(movieResponse));
            }
            catch (ServiceException exp)
            {
                return BadRequest(new ApiResponseModel(ApiResponseCode.InternalResource, exp.ErrorMessages));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseModel.Create(ApiResponseCode.InternalResource, ex.Message));
            }
        }

        [HttpPost]
        [Route("{lang}/API/WatchList")]
        public async Task<ActionResult> AddMovieToWatchListByUserId(string movieId, int userId, string lang = "en")
        {
            try
            {
                await _movieService.AddMovieToWatchListAsync(new UserWatchlistModel
                {
                     Language = lang,
                      MovieId = movieId,
                       UserId = userId
                });
                return Ok(ApiResponseModel.Create(ApiResponseCode.Success));
            }
            catch(ServiceException exp)
            {
                return BadRequest(new ApiResponseModel(ApiResponseCode.InternalResource, exp.ErrorMessages));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseModel.Create(ApiResponseCode.InternalResource,ex.Message));
            }
        }

        [HttpGet]
        [Route("API/WatchList/{userId:int}")]
        public async Task<ActionResult> GetWatchListItemsByUserAsync(int userId)
        {
            try
            {
               var watchListItems = await _movieService.GetWatchListByUserAsync(new WatchlistItemsRequestModel
                {
                     UserId = userId
                });
                return Ok(new ApiResponseModel<IEnumerable<WatchlistItemsResponseModel>>(watchListItems));
            }
            catch (ServiceException exp)
            {
                return BadRequest(new ApiResponseModel(ApiResponseCode.InternalResource, exp.ErrorMessages));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseModel.Create(ApiResponseCode.InternalResource, ex.Message));
            }
        }

        [HttpPut]
        [Route("API/WatchList")]
        public async Task<ActionResult> GetWatchListItemsByUserAsync(string movieId, int userId)
        {
            try
            {
                 await _movieService.MarkMovieAsWatchedAsync(new MarkWatchlistItemsRequestModel
                {
                     MovieId = movieId,
                      UserId = userId
                });
                return Ok(ApiResponseModel.Create(ApiResponseCode.Success));
            }
            catch (ServiceException exp)
            {
                return BadRequest(new ApiResponseModel(ApiResponseCode.InternalResource, exp.ErrorMessages));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseModel.Create(ApiResponseCode.InternalResource, ex.Message));
            }
        }
    }
}
