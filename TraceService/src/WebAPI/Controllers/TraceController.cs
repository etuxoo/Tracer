using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using TraceService.Application.Features.Tracing.Queries.GetQueries;
using TraceService.Application.Models;
using TraceService.Application.Models.Bancontact;
using AutoMapper;

namespace TraceService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/traces")] // Support path versioning
    [ApiVersion("1.0")]
    public class TraceEntitiesController(IMapper mapper) : ApiControllerBase
    {
        private readonly IMapper _mapper = mapper;

        [HttpPost("Enquiry")]
        public async Task<ActionResult<BancontactTraceModel>> TraceByTRN([FromBody][Required] string trn)
        {
            GetTraceByTrnQuery query = new() { Trn = trn };
            Result<BancontactTraceModel> result = await this.Mediator.Send(query);
            return result.Succeeded ? this.Ok(result.Data) : this.NotFound();
        }

        [HttpPost("Search")]
        public async Task<ActionResult<PaginatedList<SearchResultModel>>> Search([FromBody][Required] SearchModel model)
        {
            SearchQuery query = this._mapper.Map<SearchQuery>(model);
            Result<PaginatedList<SearchResultModel>> result = await this.Mediator.Send(query);
            return result.Succeeded ? this.Ok(result.Data.Items) : this.NotFound();
        }
    }
}
