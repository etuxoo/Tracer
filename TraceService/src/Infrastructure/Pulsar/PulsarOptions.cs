using System.ComponentModel.DataAnnotations;
using TraceService.Application.Interfaces.Options;

namespace TraceService.Infrastructure.Pulsar
{
    public class PulsarOptions : IOptions
    {
        public const string SectionName = "PulsarOptions";

        [Required]
        public string? ServiceUrl { get; set; }

        [Required]
        public string? TopicPrefix { get; set; }

    }
}
