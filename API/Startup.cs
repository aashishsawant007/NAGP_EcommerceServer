using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using API.Helpers;
using API.Middleware;
using API.Extensions;
using StackExchange.Redis;
using Infrastructure.Identity;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<EcommerceContext>(x => x.UseSqlServer("Server=IN-PG029QBM; Database=ecommerce; User ID=CPSTest;Password=Testing01; TrustServerCertificate=true"));
            services.AddDbContext<AppIdentityDbContext>(x => x.UseSqlServer("Server=IN-PG029QBM; Database=identity; User ID=CPSTest;Password=Testing01; TrustServerCertificate=true"));
            //services.AddSingleton<IConnectionMultiplexer>(c =>
            //{
            //    //var config = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"), true);
            //    //return ConnectionMultiplexer.Connect(config);
            //    return  ConnectionMultiplexer(ConfigurationOptions.Parse(""));
            //});
            services.AddAppServices();
            services.AddIdentityServices(_config);
            services.AddSwaggerDoc();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSwaggerDoc();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
