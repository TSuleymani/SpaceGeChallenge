using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceGeApp.Core.Common.Models
{
    public class MovieProviderResponse<TResponse> where TResponse : class
    {
        public IEnumerable<string> Errors { get; }
        public bool IsSuccess => !(this.Errors != null && this.Errors.Any());
        public TResponse Result { get; set; }

        public MovieProviderResponse(TResponse response, IEnumerable<string> errors)
        {
            this.Result = response;
            this.Errors = errors;
        }

        public static MovieProviderResponse<TResponse> Success<TResponse>(TResponse response) where TResponse : class
        {
            return new MovieProviderResponse<TResponse>(response, null);
        }

        public static MovieProviderResponse<TResponse> Fail<TResponse>(IEnumerable<string> errors) where TResponse : class
        {
            return new MovieProviderResponse<TResponse>(default(TResponse), errors);
        }
    }
}
