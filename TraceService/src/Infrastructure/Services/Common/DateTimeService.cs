using System;
using TraceService.Application.Interfaces;

namespace TraceService.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}
