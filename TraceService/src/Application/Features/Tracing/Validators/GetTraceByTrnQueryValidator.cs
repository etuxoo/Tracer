using FluentValidation;
using TraceService.Application.Features.Tracing.Queries.GetQueries;

namespace TraceService.Application.Features.Tracing.Validators
{
    public class GetTraceByTrnQueryValidator : AbstractValidator<GetTraceByTrnQuery>
    {
        public GetTraceByTrnQueryValidator()
        {
            this.RuleFor(x => x.Trn)
                .NotEmpty()
                .NotNull()
                .MaximumLength(50)
                .WithMessage("Trn must have value longer than 50 charecters");
        }
    }
}
