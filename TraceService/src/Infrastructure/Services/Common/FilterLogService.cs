using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TraceService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace TraceService.Infrastructure.Services
{
    public class FilterLogService : IFilterLogService
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, string> _filters;

        public FilterLogService(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._filters = new();
            this._filters.TryAdd("password", "***");
        }

        private bool HasFilteredObjects(string data)
        {
            foreach (KeyValuePair<string, string> filter in this._filters)
            {
                if (data.Contains(filter.Key))
                {
                    return true;
                }
            }

            return false;
        }

        public string Filter(string data)
        {
            string result = data;

            if (!this.HasFilteredObjects(data)) //nothing to do
            {
                return result;
            }

            try
            {
                JObject jo = JObject.Parse(data);

                foreach (KeyValuePair<string, string> filter in this._filters)
                {
                    jo = Filter(jo, filter.Key, filter.Value);
                }

                return jo.ToString();
            }
            catch (Exception)
            {
                return data;
            }
        }

        internal static JObject Filter(JObject data, string stringToFind, string stringToReplace)
        {
            if (data.ContainsKey(stringToFind))
            {
                data[stringToFind] = stringToReplace;
            }

            return data;
        }
    }
}
