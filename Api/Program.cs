using Api.Startup;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
var app = builder.Build();
app.ConfigureRequestPipeline();
app.Run();