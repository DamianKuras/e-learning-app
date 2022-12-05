using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Api.Options;
using Microsoft.AspNetCore.Builder;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Api.Startup
{
    public static class Services
    {
        public static WebApplicationBuilder AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
            AddApiVersioning(builder.Services);
            AddVersionedApiExplored(builder.Services);
            AddDatabase(builder);
            return builder;
        }
        private static void AddApiVersioning(IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        }

        private static void AddVersionedApiExplored(IServiceCollection services)
        {
            services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            });
        }

        private static void AddDatabase(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            /*builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<DataContext>();*/
        }

    }
}
