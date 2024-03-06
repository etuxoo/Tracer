using FluentValidation;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Application.Features.Tracing.Validators
{
    public class CslInBsadCommandValidator : AbstractValidator<CslInBsadCommand>
    {
        public CslInBsadCommandValidator()
        {
            this.RuleFor(x => x.Trn)
                .MaximumLength(50)
                .WithMessage("Trn is longer than 50 charecters");

            this.RuleFor(x => x.Mti)
                .MaximumLength(4)
                .WithMessage("Mti is longer than 4 charecters");

            this.RuleFor(x=>x.Tid) 
                .MaximumLength(8)
                .WithMessage("Tid is longer than 8 charecters");

            this.RuleFor(x => x.Mid)
                .MaximumLength(15)
                .WithMessage("Mid is longer than 15 charecters");

            this.RuleFor(x=>x.RrnIn) 
                .MaximumLength(128)
                .WithMessage("RrnIn is longer than 128 charecters");

            this.RuleFor(x=>x.RrnOut)
                .MaximumLength(128)
                .WithMessage("RrnOut is longer than 128 charecters");

            this.RuleFor(x=>x.ProcCode) 
                .MaximumLength(2)
                .WithMessage("ProcCode is longer than 2 charecters");

            this.RuleFor(x=>x.PanExpDate) 
                .MaximumLength(50)
                .WithMessage("PanExpDate is longer than 50 charecters");
        }
    }
}
