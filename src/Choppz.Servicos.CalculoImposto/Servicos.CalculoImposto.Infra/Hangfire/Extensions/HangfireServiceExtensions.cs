using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Servicos.CalculoImposto.Infra.Hangfire.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicos.CalculoImposto.Infra.Hangfire.Extensions
{
    public static class HangfireServiceExtensions
    {
        public static IServiceCollection AddHangfireServices(this IServiceCollection services)
        {
            services.AddHangfire(config => config.UseInMemoryStorage());
            services.AddHangfireServer();

            return services;
        }

        public static IApplicationBuilder UseHangfireJobs(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                RecurringJob.AddOrUpdate<ProcessarOutboxJob>(
                    "processar-outbox", x => x.Executar(), Cron.Minutely);
            }

            return app;
        }
    }
}
