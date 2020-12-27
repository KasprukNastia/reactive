using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using UsersLivetrackerConfigDAL.Models;
using UsersLivetrackerConfigDAL.Repos.Interfaces;

namespace SettingsProxyAPI.Controllers
{
    [Authorize,
    ApiController,
    OpenApiTag(nameof(KeywordStatisticsController), Description = "Keyword statistics"),
    Route("api/keyword-statistics")]
    public class KeywordStatisticsController : ControllerBase
    {
        private readonly IKeywordInfoRepository _keywordInfoRepository;

        public KeywordStatisticsController(IKeywordInfoRepository keywordInfoRepository)
        {
            _keywordInfoRepository = keywordInfoRepository ?? throw new ArgumentNullException(nameof(keywordInfoRepository));
        }

        [HttpGet("for-keyword"),
        Produces(MediaTypeNames.Application.Json),
        ProducesResponseType(typeof(List<KeywordBySourceItem>), StatusCodes.Status200OK)]
        public async Task<ObjectResult> GetKeywordStatistics([FromQuery] string keyword)
        {
            List<KeywordBySourceItem> result = 
                await _keywordInfoRepository.GetKeywordBySourceStatistics(keyword);
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
