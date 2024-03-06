using FluentValidation;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Application.Features.Tracing.Validators
{
    public class BancontactReceivedCommandValidator : AbstractValidator<BancontactReceivedCommand>
    {
        public BancontactReceivedCommandValidator()
        {
            this.RuleFor(x => x.Trn)
                .MaximumLength(50)
                .WithMessage("Trn is longer than 50 charecters");

            this.RuleFor(x => x.Mti)
                .MaximumLength(4)
                .WithMessage("Mti is longer than 4 charecters");
        }
    }
}
