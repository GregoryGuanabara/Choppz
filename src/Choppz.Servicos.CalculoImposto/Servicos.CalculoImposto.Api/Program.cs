using Scalar.AspNetCore;
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

var app = builder.Build();

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
