using Api.Options;
using Application.Options;
using Application.User.CommandsHandlers;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.Startup
{
    public static class Services
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
            builder.Services.AddApiVersioning();
            builder.Services.AddVersionedApiExplored();
            builder.Services.AddMediatR(typeof(RegisterUserHandler));
            builder.Services.AddAutoMapper(typeof(Program), typeof(RegisterUserHandler));
            builder.AddDatabase();
            builder.AddIdentity();
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

        private static void AddDatabase(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<DataContext>(
                options => options.UseSqlServer(connectionString)
            );
            builder.Services
                .AddIdentityCore<IdentityUser>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<DataContext>();
        }

        private static void AddIdentity(this WebApplicationBuilder builder)
        {
            var jwtOptions = new JwtOptions();
            builder.Configuration.Bind(nameof(JwtOptions), jwtOptions);
            var jwtSection = builder.Configuration.GetSection(nameof(JwtOptions));
            builder.Services.Configure<JwtOptions>(jwtSection);
            builder.Services
                .AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(jwt =>
                {
                    jwt.SaveToken = true;
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(jwtOptions.SigningKey)
                        ),
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudiences = jwtOptions.Audiences,
                        RequireExpirationTime = false,
                    };
                    jwt.Audience = jwtOptions.Audiences.First();
                    jwt.ClaimsIssuer = jwtOptions.Issuer;
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
    }
}