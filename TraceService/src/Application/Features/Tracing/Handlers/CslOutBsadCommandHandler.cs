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
    public class CslOutBsadCommandHandler(IMapper mapper,
         IRepository<CslOutBsad> repository) : IRequestHandler<CslOutBsadCommand, Result<CslOutBsadModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<CslOutBsad> _repository = repository;

        public async Task<Result<CslOutBsadModel>> Handle(CslOutBsadCommand request, CancellationToken token)
        {
            CslOutBsadModel data = this._mapper.Map<CslOutBsadModel>(request);
            CslOutBsad dataToSave = this._mapper.Map<CslOutBsad>(data);
            long count = await this._repository.AddAsync(dataToSave);
            return count == 0 ?
                Result<CslOutBsadModel>.Failure("Advice out data not saved") :
                Result<CslOutBsadModel>.Success(data);
        }
    }
}
