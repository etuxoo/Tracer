using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceService.Application.Interfaces
{
    public interface IApiKeyService
    {
        Task<bool> AuthorizeAsync(string apiKey, string apiSecret);
    }
}
