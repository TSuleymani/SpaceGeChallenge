using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpaceGeApp.Core.Common;
using SpaceGeApp.Core.Config;
using SpaceGeApp.Core.Data;
using SpaceGeApp.Core.Providers.IMDB;
using SpaceGeApp.Core.Services;
namespace SpaceGeApp.Core.Registrator
{
    public static class ServiceRegistratorExtensions
    {
        public static void AddSpaceGeAppCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SendGridOptions>(options =>
            {
                options.ApiKey = configuration["ExternalProviders:SendGrid:ApiKey"];
                options.SenderEmail = configuration["ExternalProviders:SendGrid:SenderEmail"];
                options.SenderName = configuration["ExternalProviders:SendGrid:SenderName"];
            });
            var config = configuration.GetSection(nameof(MovieServiceOptions));
            services.AddSingleton<IMDBMovieProviderOptions>(x => config.Get<IMDBMovieProviderOptions>());
            services.AddSingleton<IMovieProvider, IMDBMovieProvider>();
            services.AddSingleton<SchedulerService>();
            services.AddSingleton<IEmailSender, SendGridEmailSender>();
        }
    }
}
