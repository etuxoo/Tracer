
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TraceService.Application.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace TraceService.Infrastructure.Tasks
{
    public class WarmupServicesStartupTask : IStartupTask
    {
        private readonly IServiceCollection _services;
        private readonly IServiceProvider _provider;

        public WarmupServicesStartupTask(IServiceCollection services, IServiceProvider provider)
        {
            this._services = services;
            this._provider = provider;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (IServiceScope scope = this._provider.CreateScope())
            {
            }
            return Task.CompletedTask;
        }

        public void Execute()
        {
            using IServiceScope scope = this._provider.CreateScope();
            foreach (Type singleton in GetServices(this._services))
            {
                scope.ServiceProvider.GetServices(singleton);
            }
        }

        public static IEnumerable<Type> GetServices(IServiceCollection services)
        {
            return services
                .Where(descriptor => descriptor.ImplementationType != typeof(WarmupServicesStartupTask))
                .Where(descriptor => descriptor.ServiceType.ContainsGenericParameters == false)
                .Select(descriptor => descriptor.ServiceType)
                .Distinct();
        }
    }
}
