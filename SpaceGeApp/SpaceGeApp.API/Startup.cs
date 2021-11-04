using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpaceGeApp.Core.Common;
using SpaceGeApp.Core.Common.Abstract;
using SpaceGeApp.Core.Config;
using SpaceGeApp.Core.Data;
using SpaceGeApp.Core.Providers.IMDB;
using SpaceGeApp.Core.Services;

namespace SpaceGeApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen();

            var config = Configuration.GetSection(nameof(MovieServiceOptions));
            services.AddScoped<MovieServiceOptions>(x => config.Get<MovieServiceOptions>());
            services.AddScoped<IMDBMovieProviderOptions>(x => config.Get<IMDBMovieProviderOptions>());
            services.AddScoped<IMovieProvider, IMDBMovieProvider>();
            services.AddScoped<IMovieService, MovieService>();

            services.AddControllers();
            services.AddDbContext<MovieDbContext>(
              options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection", b => b.MigrationsAssembly("SpaceGeApp.API")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
