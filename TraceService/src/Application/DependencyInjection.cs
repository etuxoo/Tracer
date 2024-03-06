using System.Reflection;
using FluentValidation;
using MediatR;
using TraceService.Application.Behaviours;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TraceService.Application.Features.Tracing.Commands.Bancontact;
using TraceService.Application.Features.Tracing.Validators;

namespace TraceService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(SessionBehavior<,>));
            services.AddScoped<IValidator<BancontactReceivedCommand>, BancontactReceivedCommandValidator>();
            services.AddScoped<IValidator<BancontactSentCommand>, BancontactSentCommandValidator>();
            services.AddScoped<IValidator<CslInBsadCommand>, CslInBsadCommandValidator>();
            services.AddScoped<IValidator<CslInBsauCommand>, CslInBsauCommandValidator>();
            services.AddScoped<IValidator<CslOutBsadCommand>, CslOutBsadCommnadValidator>();
            services.AddScoped<IValidator<CslOutBsauCommand>, CslOutBsauCommnadValidator>();
            return services;
        }
    }
}
