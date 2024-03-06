using System.Threading.Tasks;
using MediatR;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Threading;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Domain.Entities.Bancontact;
using TraceService.Application.Features.Tracing.Commands.Bancontact;

namespace TraceService.Application.Features.Tracing.Handlers
{
    public class CslOutBsauCommandHandler(IMapper mapper,
         IRepository<CslOutBsau> repository) : IRequestHandler<CslOutBsauCommand, Result<CslOutBsauModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<CslOutBsau> _repository = repository;

        public async Task<Result<CslOutBsauModel>> Handle(CslOutBsauCommand request, CancellationToken token)
        {
            CslOutBsauModel data = this._mapper.Map<CslOutBsauModel>(request);
            CslOutBsau dataToSave = this._mapper.Map<CslOutBsau>(data);
            long count = await this._repository.AddAsync(dataToSave);
            return count == 0 ?
                Result<CslOutBsauModel>.Failure("Authorization out data not saved") :
                Result<CslOutBsauModel>.Success(data);
        }
    }
}
