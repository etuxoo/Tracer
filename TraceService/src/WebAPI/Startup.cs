using CorrelationId;
using CorrelationId.DependencyInjection;
using TraceService.Application;
using TraceService.Infrastructure;
using TraceService.WebAPI.Binders;
using TraceService.WebAPI.Filters;
using TraceService.WebAPI.Middlewares;
using TraceService.WebAPI.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TraceService.WebAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();

            services.AddInfrastructure(this.Configuration);

            services.AddHttpContextAccessor();

            services.AddCorrelationId().WithTraceIdentifierProvider();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c=> c.SchemaFilter<SwaggerExcludeFilter>());
            services.AddSwaggerGenNewtonsoftSupport();

            services.AddMemoryCache();

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddControllers(options => {
                options.ModelBinderProviders.Insert(0, new CustomDateTimeModelBinderProvider());
                options.Filters.Add<ApiExceptionFilterAttribute>();
                   
            })
            .AddNewtonsoftJson(options =>
            {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    //options.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
                    options.SerializerSettings.ReferenceLoopHandling =  Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter()));
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (!env.IsProduction())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    // build a swagger endpoint for each discovered API version
                    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                    };
                    
                });
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseCorrelationId();
            app.UseSerilogRequestLogging(
                options =>
                    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => diagnosticContext.Set("CorrelationId", httpContext.Request.Headers["X-Correlation-ID"]));
            app.UseMiddleware<SerilogMiddleware>();

            app.UseRouting();

            //app.UseAuthentication();

            //app.UseAuthorization();
            
            //if (env.IsDevelopment() == false)
            //{
            //    app.UseMiddleware<ApiKeyMiddleware>();
            //}

            //app.UseMiddleware<TimeZoneMiddleware>();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
