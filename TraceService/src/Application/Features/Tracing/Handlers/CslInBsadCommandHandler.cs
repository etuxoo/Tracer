using MediatR;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Models;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Domain.Entities.Bancontact;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Application.Features.Tracing.Handlers
{
    public class CslInBsadCommandHandler(IMapper mapper,
         IRepository<CslInBsad> repository) : IRequestHandler<CslInBsadCommand, Result<CslInBsadModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<CslInBsad> _repository = repository;

        public async Task<Result<CslInBsadModel>> Handle(CslInBsadCommand request, CancellationToken token)
        {
            CslInBsadModel data = this._mapper.Map<CslInBsadModel>(request);
            CslInBsad dataToSave = this._mapper.Map<CslInBsad>(data);
            long count = await this._repository.AddAsync(dataToSave);
            return count == 0 ?
                Result<CslInBsadModel>.Failure("Advice in data not saved") :
                Result<CslInBsadModel>.Success(data);
        }
    }
}
