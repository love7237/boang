using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using T.Test.WebApi.Models;
using T.Utility.Algorithms;
using T.Utility.Protocol;

namespace T.Test.WebApi.Controllers
{
    [Route("")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly ILogger<AlgorithmController> _logger;
        private readonly AlgorithmHttpClient _algorithmHttpClient;

        public AlgorithmController(ILogger<AlgorithmController> logger, AlgorithmHttpClient algorithmHttpClient)
        {
            _logger = logger;
            _algorithmHttpClient = algorithmHttpClient;
        }

        /// <summary>
        /// 车牌识别
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("lpc/plate/ocr")]
        public async Task<ActionContent<List<PlateOcrContent>>> OcrPlatesAsync(ImageData data)
        {
            return await _algorithmHttpClient.OcrPlatesAsync(Guid.NewGuid().ToString(), data.ImageSource, data.ImageType);
        }

        /// <summary>
        /// 九类检测
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("ai/multidetect/multiDetect")]
        public async Task<ActionContent<List<MulticlassContent>>> MultiDetectObjectsAsync(ImageData data)
        {
            return await _algorithmHttpClient.DetectObjectsAsync(Guid.NewGuid().ToString(), data.ImageSource, data.ImageType);
        }

        /// <summary>
        /// 基于九类检测的车型识别
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost("ai/multidetect/carmodels")]
        public async Task<ActionContent<List<CarModelExtendContent>>> MultiDetectModelsAsync(ImageData data)
        {
            return await _algorithmHttpClient.DetectModelsAsync(Guid.NewGuid().ToString(), data.ImageSource, data.ImageType);
        }

        /// <summary>
        /// 基于车牌顶点的车脸矫正
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ai/multidetect/carFaceByPlatePoints")]
        public async Task<ActionContent<CarFaceCorrectContent>> CarFaceCorrectAsync(FaceCorrectRequest request)
        {
            return await _algorithmHttpClient.CorrectCarFaceAsync(Guid.NewGuid().ToString(), request.ImageSource, request.ImageType, request.PlatePoints);
        }

        /// <summary>
        /// 基于矫正的车脸图或指定九类检测车脸区域的特写图的车型识别
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ai/carmodel/carmodel")]
        public async Task<ActionContent<CarModelSimpleContent>> CarFaceModelAsync(FaceModelRequest request)
        {
            Rectangle? rectangle = null;

            if (request.FacePoints != null && request.FacePoints.Count == 4)
            {
                rectangle = new Rectangle(request.FacePoints[0], request.FacePoints[1], request.FacePoints[2] - request.FacePoints[0], request.FacePoints[3] - request.FacePoints[1]);
            }

            return await _algorithmHttpClient.GetFaceModelAsync(Guid.NewGuid().ToString(), request.ImageSource, request.ImageType, rectangle);
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
