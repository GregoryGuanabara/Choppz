using Hangfire;
using Scalar.AspNetCore;
using Serilog;
using Servicos.CalculoImposto.Api.Middlewares;
using Servicos.CalculoImposto.Application;
using Servicos.CalculoImposto.Core;
using Servicos.CalculoImposto.Infra;
using Servicos.CalculoImposto.Infra.Hangfire.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services
    .AddInfrastructureModuleServices()
    .AddCoreModuleServices()
    .AddApplicationModuleServices();

builder.Services.AddOpenApi();

builder.Services.AddOptions<ScalarOptions>().BindConfiguration("ScalarOptions");

builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .Enrich.FromLogContext());

builder.Services.AddHangfireServices();

var app = builder.Build();

if (!app.Environment.IsEnvironment("Test"))
{
    app.UseHangfireDashboard();
    app.UseHangfireJobs();
}

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

public partial class Program
{
}
