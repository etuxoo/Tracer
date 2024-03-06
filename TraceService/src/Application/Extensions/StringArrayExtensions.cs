
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceService.Application.Extensions
{
    public static class StringArrayExtensions
    {
        public static string TryGetValue(this string[] t, int index, string defaultValue)
        {
            return (index >= t.Length) ? defaultValue:  t[index];
        }
    }
}
