using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpaceGeApp.Core.Common.Models;

namespace SpaceGeApp.Core.Common
{
   public interface IMovieProvider
    {
        Task<MovieProviderResponse<SearchData>> GetMovieByNameAsync(SearchRequest searchRequest);
        Task<MovieProviderResponse<ReviewData>> GetMovieByIdAsync(ReviewRequest reviewRequest);
        Task<MovieProviderResponse<MovieDetailsData>> GetMovieDetailedDescriptionAsync(MovieDetailsRequest movieDetailsRequest);
    }
}
