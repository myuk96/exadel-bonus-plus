using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ExadelBonusPlus.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace ExadelBonusPlus.WebApi.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        private readonly IVendorService _vendorService;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(ILogger<HistoryController> logger,
                                IVendorService vendorService, 
                                IHistoryService historyService)
        {
            _historyService = historyService;
            _vendorService = vendorService;
            _logger = logger;
        }

        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Add history", Type = typeof(ResultDto<HistoryDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> AddHistory([FromBody] AddHistoryDTO history)
        {
            var result = await _historyService.AddHistory(history);
            return Ok(result);
        }
        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Delete history by id", Type = typeof(ResultDto<HistoryDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult<HistoryDto>> DeleteHistoryAsync([FromRoute] Guid id)
        {
            var result = await _historyService.DeleteHistory(id);
            return Ok(result);
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get all history", Type = typeof(ResultDto<List<HistoryDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> GetAllHistory()
        {
            var result = await _historyService.GetAllHistory();
            return Ok(result);
        }

        [HttpGet]
        [Route(("users/{userId:Guid}"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "get user history on period ", Type = typeof(ResultDto<List<UserHistoryDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> GetUserHistoryByDate([FromRoute] Guid userId, DateTime datestart, DateTime dateEnd)
        {
            var result = await _historyService.GetUserHistoryByUsageDate(userId, datestart, dateEnd);
            return Ok(result);
        }
        [HttpGet]
        [Route(("bonuses/{bonusId:Guid}"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get bonus history on period ", Type = typeof(ResultDto<List<BonusHistoryDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> GetBonusHistoryByDate([FromRoute] Guid bonusId, DateTime datestart, DateTime dateEnd)
        {
            var result = await _historyService.GetBonusHistoryByUsageDate(bonusId, datestart, dateEnd);
            return Ok(result);
        }

        [HttpPut]
        [Route(("{historyId:Guid}/estimate"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Estimate usage bonus", Type = typeof(ResultDto<UserHistoryDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> EstimateBonus([FromRoute] Guid historyId, int estimate)
        {
            var result = await _historyService.EstimateBonus(historyId, estimate, CancellationToken.None);
            return Ok(result);
        }




    }
}
