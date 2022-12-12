using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Api.Options;
using Microsoft.AspNetCore.Builder;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Application.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MediatR;
using Application.User.CommandsHandlers;

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
            builder.Services.AddApiVersioning();
            builder.Services.AddVersionedApiExplored();
            builder.Services.AddMediatR(typeof(RegisterUserHandler));
            builder.Services.AddAutoMapper(typeof(Program),typeof(RegisterUserHandler));
            builder.AddDatabase();
            return builder;
        }
        private static void AddApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ApiVersionReader = new UrlSegmentApiVersionReader();
            });
        }

        private static void AddVersionedApiExplored(this IServiceCollection services)
        {
            services.AddVersionedApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            });
        }

        private static void AddDatabase(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
            builder.Services.AddIdentityCore<IdentityUser>()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}
