using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using T.Test.WebApi.Models;
using T.Utility.Algorithms;
using T.Utility.Protocol;
using T.Utility.Snowflake;

namespace T.Test.WebApi.Controllers
{
    [Route("")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly ILogger<AlgorithmController> _logger;
        private readonly AlgorithmHttpClient _algorithmHttpClient;
        private readonly SnowflakeHelper _snowflakeHelper;

        public AlgorithmController(ILogger<AlgorithmController> logger, AlgorithmHttpClient algorithmHttpClient, SnowflakeHelper snowflakeHelper)
        {
            _logger = logger;
            _algorithmHttpClient = algorithmHttpClient;
            _snowflakeHelper = snowflakeHelper;
        }

        /// <summary>
        /// 车牌识别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("lpc/plate/ocr")]
        public async Task<ActionContent<List<PlateOcrContent>>> OcrPlatesAsync(AlgorithmRequest request)
        {
            return await _algorithmHttpClient.OcrPlatesAsync(Guid.NewGuid().ToString(), request.ImageSource, request.ImageType);
        }

        /// <summary>
        /// 九类检测
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ai/multidetect/multiDetect")]
        public async Task<ActionContent<List<MulticlassContent>>> DetectObjectsAsync(AlgorithmRequest request)
        {
            return await _algorithmHttpClient.DetectObjectsAsync(Guid.NewGuid().ToString(), request.ImageSource, request.ImageType);
        }

        /// <summary>
        /// 车型识别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ai/carmodel/carmodel")]
        public async Task<ActionContent<List<CarModelExtendContent>>> DetectModelsAsync(AlgorithmRequest request)
        {
            return await _algorithmHttpClient.DetectModelsAsync(Guid.NewGuid().ToString(), request.ImageSource, request.ImageType);
        }

        /// <summary>
        /// 泊位历史状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("bscache/berth/history")]
        public async Task<ActionContent<BerthHistoryStateContent>> BerthHistoryStatesAsync(BerthHistoryStateRequest request)
        {
            return await _algorithmHttpClient.BerthHistoryStatesAsync("http://123.57.76.88:20031/alg/19", "bc98870e-c29a-11ea-bb35-00ff98b19e27", request.ParkCode, request.BerthCode, request.BeginTime, request.EndTime);
        }

    }
}
