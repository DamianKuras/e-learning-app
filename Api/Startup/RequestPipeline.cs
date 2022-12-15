using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Api.Startup
{
    public static class RequestPipeline
    {
        public static WebApplication ConfigureRequestPipeline(this WebApplication app)
        {
            ConfigureSwagger(app);
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            return app;
        }


        private static WebApplication ConfigureSwagger(WebApplication app)
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
            return app;
        }
    }
}
