using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Models;
using TraceService.Domain.Entities.Bancontact;
using MediatR;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Application.Features.Tracing.Handlers
{
    public class BancontactSentCommandHandler(IMapper mapper,
         IRepository<BancontactSent> repository) : IRequestHandler<BancontactSentCommand, Result<BancontactSentModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<BancontactSent> _repository = repository;

        public async Task<Result<BancontactSentModel>> Handle(BancontactSentCommand request, CancellationToken token)
        {
            BancontactSentModel data = this._mapper.Map<BancontactSentModel>(request);
            BancontactSent dataToSave = this._mapper.Map<BancontactSent>(data);
            long count = await this._repository.AddAsync(dataToSave);
            return count == 0 ?
                Result<BancontactSentModel>.Failure("Bancontact Sent data not saved") :
                Result<BancontactSentModel>.Success(data);
        }
    }
}
