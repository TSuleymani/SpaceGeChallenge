using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SpaceGeApp.Core.Common;
using SpaceGeApp.Core.Data;
using SpaceGeApp.Core.Providers.IMDB;
using SpaceGeApp.Core.Registrator;
using SpaceGeApp.Core.Services;

namespace SpaceGeApp.Scheduler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSpaceGeAppCore(hostContext.Configuration);
                    services.AddDbContext<MovieDbContext>(
           options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection", b => b.MigrationsAssembly("SpaceGeApp.API")), ServiceLifetime.Singleton);
                    services.AddHostedService<Worker>();
                });
    }
}
