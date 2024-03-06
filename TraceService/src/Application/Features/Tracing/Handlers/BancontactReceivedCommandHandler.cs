using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using TraceService.Application.Models;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Domain.Entities.Bancontact;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Application.Features.Tracing.Handlers
{
    public class BancontactReceivedCommandHandler(IMapper mapper,
         IRepository<BancontactReceived> repository) : IRequestHandler<BancontactReceivedCommand, Result<BancontactReceivedModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<BancontactReceived> _repository = repository;

        public async Task<Result<BancontactReceivedModel>> Handle(BancontactReceivedCommand request, CancellationToken token)
        {
            BancontactReceivedModel data = this._mapper.Map<BancontactReceivedModel>(request);
            BancontactReceived dataToSave = this._mapper.Map<BancontactReceived>(data);
            long count = await this._repository.AddAsync(dataToSave);
            return count == 0 ?
                Result<BancontactReceivedModel>.Failure("BancontactReceived data not saved") :
                Result<BancontactReceivedModel>.Success(data);
        }
    }  
}
