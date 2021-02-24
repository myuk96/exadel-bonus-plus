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

namespace ExadelBonusPlus.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        [Route(("user/{userId:Guid}"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "get all user history", Type = typeof(ResultDto<List<UserHistoryDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> GetUserHistory([FromRoute]Guid userId)
        {
            var result = await _historyService.GetUserAllHistory(userId);
            return Ok(result);
        }

        [HttpGet]
        [Route(("user/{userId:Guid}/date"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "get user history on period ", Type = typeof(ResultDto<List<UserHistoryDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> GetUserHistoryByDate([FromRoute] Guid userId, DateTime start, DateTime end)
        {
            var result = await _historyService.GetUserHistoryByUsageDate(userId, start, end);
            return Ok(result);
        }
        [HttpGet]
        [Route(("bonus/{bonusId:Guid}"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get all bonus history", Type = typeof(ResultDto<List<BonusHistoryDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> GetBonusHistory([FromRoute] Guid bonusId)
        {
            var result = await _historyService.GetBonusAllHistory(bonusId);
            return Ok(result);
        }
        [HttpGet]
        [Route(("bonus/{bonusId:Guid}/date"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Get bonus history on period ", Type = typeof(ResultDto<List<BonusHistoryDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> GetBonusHistoryByDate([FromRoute] Guid bonusId, DateTime start, DateTime end)
        {
            var result = await _historyService.GetBonusHistoryByUsageDate(bonusId, start, end);
            return Ok(result);
        }

        [HttpPut]
        [Route(("estimate/{historyId:Guid}"))]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Estimate usage bonus", Type = typeof(ResultDto<UserHistoryDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<HistoryDto>> EstimateBonus([FromRoute] Guid historyId, int estimate)
        {
            var result = await _historyService.EstimateBonus(historyId, estimate, CancellationToken.None);
            return Ok(result);
        }




    }
}
