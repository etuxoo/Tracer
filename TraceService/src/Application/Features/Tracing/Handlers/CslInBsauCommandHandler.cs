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
    public class CslInBsauCommandHandler(IMapper mapper,
         IRepository<CslInBsau> repository) : IRequestHandler<CslInBsauCommand, Result<CslInBsauModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<CslInBsau> _repository = repository;

        public async Task<Result<CslInBsauModel>> Handle(CslInBsauCommand request, CancellationToken token)
        {
            CslInBsauModel data = this._mapper.Map<CslInBsauModel>(request);
            CslInBsau dataToSave = this._mapper.Map<CslInBsau>(data);
            long count = await this._repository.AddAsync(dataToSave);
            return count == 0 ?
                Result<CslInBsauModel>.Failure("Authorization in data not saved") :
                Result<CslInBsauModel>.Success(data);
        }
    }
}
