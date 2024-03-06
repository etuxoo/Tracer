using System;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Threading.Tasks;

[DisallowConcurrentExecution]
public class HelloWorldJob : IJob
{
    private readonly ILogger<HelloWorldJob> _logger;
    public HelloWorldJob(ILogger<HelloWorldJob> logger)
    {
        this._logger = logger;
    }

    public Task Execute(IJobExecutionContext context)
    {
        this._logger.LogInformation("Hello world!");
        return Task.CompletedTask;
    }
}
