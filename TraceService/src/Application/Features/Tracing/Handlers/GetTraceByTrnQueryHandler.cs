using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using TraceService.Application.Interfaces.Repositories;
using TraceService.Application.Models.Bancontact;
using TraceService.Application.Models;
using TraceService.Domain.Entities.Bancontact;
using MediatR;
using TraceService.Application.Features.Tracing.Queries.GetQueries;

namespace TraceService.Application.Features.Tracing.Handlers
{
    public class GetTraceByTrnQueryHandler(IMapper mapper, IRepository<BancontactReceived> brrepository, IRepository<BancontactSent> bsrepository,
          IRepository<CslInBsad> iadrepository, IRepository<CslInBsau> iaurepository, IRepository<CslOutBsad> oadrepository, IRepository<CslOutBsau> oaurepository) : IRequestHandler<GetTraceByTrnQuery, Result<BancontactTraceModel>>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IRepository<BancontactReceived> _brRepository = brrepository;
        private readonly IRepository<BancontactSent> _bsRepository = bsrepository;
        private readonly IRepository<CslInBsad> _iadRepository = iadrepository;
        private readonly IRepository<CslInBsau> _iauRepository = iaurepository;
        private readonly IRepository<CslOutBsad> _oadRepository = oadrepository;
        private readonly IRepository<CslOutBsau> _oauRepository = oaurepository;

        public async Task<Result<BancontactTraceModel>> Handle(GetTraceByTrnQuery request, CancellationToken token)
        {
            BancontactTraceModel result = new()
            {
                BancontactReceived = this._mapper.Map<BancontactReceivedModel>(await this._brRepository.GetByTrnAsync(request.Trn)),
                BancontactSent= this._mapper.Map<BancontactSentModel>(await this._bsRepository.GetByTrnAsync(request.Trn)),
                CslInBsad= this._mapper.Map<CslInBsadModel>(await this._iadRepository.GetByTrnAsync(request.Trn)),
                CslInBsau= this._mapper.Map<CslInBsauModel>(await this._iauRepository.GetByTrnAsync(request.Trn)),
                CslOutBsad= this._mapper.Map<CslOutBsadModel>(await this._oauRepository.GetByTrnAsync(request.Trn)),
                CslOutBsau= this._mapper.Map<CslOutBsauModel>(await this._oadRepository.GetByTrnAsync(request.Trn)),
            };
            return result == null ?
                Result<BancontactTraceModel>.Failure("Missing trace data") :
                Result<BancontactTraceModel>.Success(result);
        }
    }
}
