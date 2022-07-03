using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Shopperholics.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shopperholics.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Shopperholics.Models;
using Microsoft.Extensions.Azure;
using Azure.Storage.Queues;
using Azure.Storage.Blobs;
using Azure.Core.Extensions;



namespace Shopperholics
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 7;
                options.Password.RequireUppercase = true;

                options.User.RequireUniqueEmail = true;
            })
              .AddEntityFrameworkStores<ShopperholicsContext>();
            services.AddTransient<IShopperholicsRepository, ShopperholicsRepository>();
           // string connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=shopperholics;Trusted_Connection=True;MultipleActiveResultSets=true";
            services.AddDbContext<ShopperholicsContext>(options =>
               options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc(option => option.EnableEndpointRouting = false).AddSessionStateTempDataProvider();
              
            services.AddSession();
        /*    services.AddAzureClients(builder =>
            {
                builder.AddBlobServiceClient(Configuration["ConnectionStrings:DefaultConnection:blob"], preferMsi: true);
                builder.AddQueueServiceClient(Configuration["ConnectionStrings:DefaultConnection:queue"], preferMsi: true);
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseSession();
         
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "ShopperholicsRoute",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Products", action = "Index" },
                    constraints: new { id = "[0-9]+" });
            });

        }
    }
    internal static class StartupExtensions
    {
        public static IAzureClientBuilder<BlobServiceClient, BlobClientOptions> AddBlobServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddBlobServiceClient(serviceUri);
            }
            else
            {
                return builder.AddBlobServiceClient(serviceUriOrConnectionString);
            }
        }
        public static IAzureClientBuilder<QueueServiceClient, QueueClientOptions> AddQueueServiceClient(this AzureClientFactoryBuilder builder, string serviceUriOrConnectionString, bool preferMsi)
        {
            if (preferMsi && Uri.TryCreate(serviceUriOrConnectionString, UriKind.Absolute, out Uri serviceUri))
            {
                return builder.AddQueueServiceClient(serviceUri);
            }
            else
            {
                return builder.AddQueueServiceClient(serviceUriOrConnectionString);
            }
        }
    }
}
