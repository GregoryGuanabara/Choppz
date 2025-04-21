using Scalar.AspNetCore;
using Serilog;
using Servicos.CalculoImposto.Api.Middlewares;
using Servicos.CalculoImposto.Application;
using Servicos.CalculoImposto.Core;
using Servicos.CalculoImposto.Infra;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddCoreModuleServices()
    .AddApplicationModuleServices()
    .AddInfrastructureModuleServices();

builder.Services.AddOpenApi();

builder.Services.AddOptions<ScalarOptions>().BindConfiguration("ScalarOptions");

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .Enrich.FromLogContext());

var app = builder.Build();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/scalar/v1");
        return;
    }
    await next();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
