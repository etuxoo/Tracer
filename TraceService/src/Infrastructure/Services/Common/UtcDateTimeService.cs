using System;
using TraceService.Application.Interfaces;

namespace TraceService.Infrastructure.Services;

public class 
    UtcDateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
