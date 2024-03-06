using FluentValidation;
using TraceService.Application.Features.Tracing.Queries.GetQueries;

namespace TraceService.Application.Features.Tracing.Validators
{
    public class SearchQueryValidator : AbstractValidator<SearchQuery>
    {
        
        public SearchQueryValidator() 
        {

        }
    }
}
