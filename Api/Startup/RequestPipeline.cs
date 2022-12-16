using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Api.Startup
{
    public static class RequestPipeline
    {
        public static WebApplication ConfigureRequestPipeline(this WebApplication app)
        {
            app.ConfigureSwagger();
            app.ConfigureGlobalExceptionHandler();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }

        private static void ConfigureGlobalExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "aplication/json";
                    await context.Response.WriteAsJsonAsync("Something went wrong. Please try again latter");
                });
            });
        }

        private static void ConfigureSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.ApiVersion.ToString());
                    }
                });
            }
        }
    }
}