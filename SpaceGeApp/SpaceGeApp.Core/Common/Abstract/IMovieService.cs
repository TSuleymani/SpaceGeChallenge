using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpaceGeApp.Core.Common.Models;

namespace SpaceGeApp.Core.Common.Abstract
{
    public interface IMovieService
    {
        Task<SearchResponseModel> GetMoviesByNameAsync(SearchRequestModel searchRequest);
        Task<UserWatchlistResponseModel> AddMovieToWatchListAsync(UserWatchlistModel userWatchlist);
        Task<IEnumerable<WatchlistItemsResponseModel>> GetWatchListByUserAsync(WatchlistItemsRequestModel watchlistItemsRequest);
        Task<bool> MarkMovieAsWatchedAsync(MarkWatchlistItemsRequestModel watchlistItemsRequest);
    }
}
