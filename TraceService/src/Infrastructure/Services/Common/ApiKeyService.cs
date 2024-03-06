using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TraceService.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TraceService.Infrastructure.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<ApiKeyService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly SemaphoreSlim _semaphoreSlim = new(1);
        //private readonly string _cacheKey = "api-key-secrets";
        //private readonly int _expirationPeriod = 5;

        public ApiKeyService(IMapper mapper, IMemoryCache memoryCache, ILogger<ApiKeyService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this._mapper = mapper;
            this._memoryCache = memoryCache;
            this._logger = logger;
            this._serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<bool> AuthorizeAsync(string apiKey, string apiSecret)
        {
            //if (!this._memoryCache.TryGetValue(this._cacheKey, out List<WebApiAuthentication> keys))
            //{
            //    await this.semaphoreSlim.WaitAsync();
            //    try
            //    {
            //        if (!this._memoryCache.TryGetValue(this._cacheKey, out keys))
            //        {
            //            using (IServiceScope scope = this._serviceScopeFactory.CreateScope())
            //            {
            //                IApplicationDbContext context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            //                keys = context.WebApiAuthentications.Select(x => x).AsNoTracking().ToList();
            //                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
            //                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(this._expirationPeriod));
            //                this._memoryCache.Set(this._cacheKey, keys, cacheEntryOptions);
            //            }
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        this._logger.LogError(e.Message);
            //    }
            //    finally
            //    {
            //        this.semaphoreSlim.Release();
            //    }
            //}

            //if (string.IsNullOrEmpty(apiKey) == false)
            //{
            //    WebApiAuthentication key = keys.FirstOrDefault(x => x.ApiKey == apiKey);
            //    return key != null && key.ApiSecret == apiSecret;
            //}

            return true;
        }
    }
}
