
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraceService.Application.Extensions
{
    public static class ListExtensions
    {
        public static IList<T> ToIList<T>(this List<T> t)
        {
            return t;
        }
    }
}
