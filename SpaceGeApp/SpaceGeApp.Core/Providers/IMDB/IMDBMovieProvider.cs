using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpaceGeApp.Core.Common;
using SpaceGeApp.Core.Common.Models;
using SpaceGeApp.Core.Exceptions;

namespace SpaceGeApp.Core.Providers.IMDB
{
    public class IMDBMovieProvider : IMovieProvider
    {
        private readonly IMDBMovieProviderOptions _options;
        public IMDBMovieProvider(IMDBMovieProviderOptions options)
        {
            this._options = options ?? throw new ArgumentNullException(nameof(IMDBMovieProvider));
        }

        public async Task<MovieProviderResponse<SearchData>> GetMovieByNameAsync(SearchRequest searchRequest)
        {
            var client = CreateHttpClient();

            try
            {
                var requestUri = $"{this._options.BaseUrl}/{searchRequest.Language}/API/Search/{this._options.ApiKey}/{searchRequest.Expression}";
                var response = await client.GetAsync(requestUri);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();

                    var responseObj = JsonConvert.DeserializeObject<SearchData>(responseStr);

                    return MovieProviderResponse<SearchData>.Success<SearchData>(responseObj);
                }
                else
                {
                    var responseStr = await response.Content.ReadAsStringAsync();

                    return MovieProviderResponse<SearchData>.Fail<SearchData>(new List<string>() { "Occur error!" });
                }
            }
            catch (Exception ex)
            {
                throw new ProviderException("Occur error while connecting to provider!");
            }
            finally
            {
                client?.Dispose();
            }
        }

        public async Task<MovieProviderResponse<ReviewData>> GetMovieByIdAsync(ReviewRequest reviewRequest)
        {
            var client = CreateHttpClient();

            try
            {
                var requestUri = $"{this._options.BaseUrl}/{reviewRequest.Language}/API/Reviews/{this._options.ApiKey}/{reviewRequest.Id}";
                var response = await client.GetAsync(requestUri);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();

                    var responseObj = JsonConvert.DeserializeObject<ReviewData>(responseStr);

                    return MovieProviderResponse<ReviewData>.Success<ReviewData>(responseObj);
                }
                else
                {
                    var responseStr = await response.Content.ReadAsStringAsync();

                    return MovieProviderResponse<ReviewData>.Fail<ReviewData>(new List<string>() { "Occur error!" });
                }
            }
            catch (Exception ex)
            {
                throw new ProviderException("Occur error while connecting to provider!");
            }
            finally
            {
                client?.Dispose();
            }
        }

        private HttpClient CreateHttpClient()
        {
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(_options.BaseUrl),
            };
            return client;
        }

        public async Task<MovieProviderResponse<MovieDetailsData>> GetMovieDetailedDescriptionAsync(MovieDetailsRequest movieDetailsRequest)
        {
            var client = CreateHttpClient();

            try
            {
                var requestUri = $"{this._options.BaseUrl}/{movieDetailsRequest.Language}/API/Title/{this._options.ApiKey}/{movieDetailsRequest.MovieId}";
                var response = await client.GetAsync(requestUri);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseStr = await response.Content.ReadAsStringAsync();

                    var responseObj = JsonConvert.DeserializeObject<MovieDetailsData>(responseStr);

                    return MovieProviderResponse<MovieDetailsData>.Success<MovieDetailsData>(responseObj);
                }
                else
                {
                    var responseStr = await response.Content.ReadAsStringAsync();

                    return MovieProviderResponse<MovieDetailsData>.Fail<MovieDetailsData>(new List<string>() { "Occur error!" });
                }
            }
            catch (Exception ex)
            {
                throw new ProviderException("Occur error while connecting to provider!");
            }
            finally
            {
                client?.Dispose();
            }
        }
    }
}
