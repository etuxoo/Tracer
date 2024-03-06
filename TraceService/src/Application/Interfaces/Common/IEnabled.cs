using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace TraceService.Application.Interfaces.Common
{
    public interface IEnabled
    {
        public bool IsEnabled { get; set; }
    }
}
