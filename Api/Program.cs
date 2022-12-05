using Api.Options;
using Api.Startup;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;

var builder = WebApplication.CreateBuilder(args);
builder.AddServices();
var app = builder.Build();
app.ConfigureRequestPipeline();
app.Run();
