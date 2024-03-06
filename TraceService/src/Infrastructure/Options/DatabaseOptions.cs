using System.ComponentModel.DataAnnotations;
using TraceService.Application.Interfaces.Options;

namespace TraceService.Infrastructure.Options
{
    public class DatabaseOptions : IOptions
    {
        public const string SectionName = nameof(DatabaseOptions);

        [Required]
        public string ConnectionString { get; set; }
    }
}
