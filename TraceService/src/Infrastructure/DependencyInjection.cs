using TraceService.Application.Interfaces;
using TraceService.Infrastructure.Handlers;
using TraceService.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Reflection;
using System.Text;
using TraceService.Infrastructure.Tasks;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Infrastructure.Persistence.Repositories;
using TraceService.Application.Interfaces.Utils;
using TraceService.Infrastructure.Services.Utils;
using TraceService.Infrastructure.Options;
using TraceService.Infrastructure.Pulsar;
using TraceService.Application.Interfaces.Pulsar;
using TraceService.Infrastructure.Subscribers;
using TraceService.Infrastructure.Interfaces.Pulsar;
using TraceService.Domain.Entities.Bancontact;
using System.Data;
using Npgsql;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterOptions(services);

            Assembly assembly = Assembly.GetExecutingAssembly();
            services.AddAutoMapper(assembly);
            //services.AddJwtBearerTokenAuthentication(configuration);
            services.AddScoped<IDateTime, UtcDateTimeService>();
            services.AddTransient<IDateTime, UtcDateTimeService>();
            services.AddTransient<IFilterLogService, FilterLogService>();
            services.AddSingleton<IStartupTask>(provider => new WarmupServicesStartupTask(services, provider));
            services.AddTransient<IEncryptionService, EncryptionService>();
            services.AddTransient<IMessageSubscriber<BancontactReceivedCommand>,PulsarMessageSubscriber<BancontactReceivedCommand>>();
            services.AddTransient<IMessageSubscriber<BancontactSentCommand>, PulsarMessageSubscriber<BancontactSentCommand>>();
            services.AddTransient<IMessageSubscriber<CslInBsadCommand>, PulsarMessageSubscriber<CslInBsadCommand>>();
            services.AddTransient<IMessageSubscriber<CslInBsauCommand>, PulsarMessageSubscriber<CslInBsauCommand>>();
            services.AddTransient<IMessageSubscriber<CslOutBsadCommand>, PulsarMessageSubscriber<CslOutBsadCommand>>();
            services.AddTransient<IMessageSubscriber<CslOutBsauCommand>, PulsarMessageSubscriber<CslOutBsauCommand>>();
            services.AddTransient<ISerializer,PulsarSerializer>();

            #region Bancontact

            services.AddTransient<IRepository<BancontactSent>,BancontactSentRepository>();
            services.AddTransient<IRepository<BancontactReceived>,BancontactReceivedRepository>();
            services.AddTransient<IRepository<CslInBsad>,CslInBsadRepository>();
            services.AddTransient<IRepository<CslInBsau>,CslInBsauRepository>();
            services.AddTransient<IRepository<CslOutBsad>,CslOutBsadRepository>();
            services.AddTransient<IRepository<CslOutBsau>, CslOutBsauRepository>();
            services.AddTransient<ISearchRepository, BancontactSearchRepository>();

            services.AddHostedService<TraceSubscriber<BancontactReceivedCommand>>();
            services.AddHostedService<TraceSubscriber<BancontactSentCommand>>();
            services.AddHostedService<TraceSubscriber<CslInBsadCommand>>();
            services.AddHostedService<TraceSubscriber<CslInBsauCommand>>();
            services.AddHostedService<TraceSubscriber<CslOutBsadCommand>>();
            services.AddHostedService<TraceSubscriber<CslOutBsauCommand>>();

            #endregion

            services.AddTransient<ApmDelegatingHandler>();
            services.AddSingleton<IApiKeyService, ApiKeyService>();



            // other config

            //services.AddApiVersioning(options =>
            //{
            //    options.DefaultApiVersion = new(1, 0);
            //    options.AssumeDefaultVersionWhenUnspecified = false;
            //    options.ReportApiVersions = true;
            //});

            //services.AddVersionedApiExplorer(options =>
            //{
            //    options.GroupNameFormat = "'v'VVV";
            //    options.SubstituteApiVersionInUrl = true;
            //});


            return services;
        }

        private static void RegisterOptions(IServiceCollection services)
        {
            services.AddOptions<PulsarOptions>()
                .BindConfiguration(PulsarOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<DatabaseOptions>()
                .BindConfiguration(DatabaseOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        public static IServiceCollection AddJwtBearerTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            string issuerSigningKey = configuration.GetValue<string>("JwtBearerTokenAuthentication:IssuerSigningKey");
            byte[] issuerSigningKeyData = Encoding.ASCII.GetBytes(issuerSigningKey);

            services
                .AddAuthentication(authentication =>
                {
                    authentication.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    authentication.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(issuerSigningKeyData),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            return services;
        }

       

    }
}
