using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using T.Utility.Extensions;
using T.Utility.Http;
using T.Utility.Protocol;
using T.Utility.Serialization;

namespace T.Utility.Algorithms
{
    /// <summary>
    /// 算法服务封装HttpClient
    /// </summary>
    public class AlgorithmHttpClient
    {
        private readonly ILogger<AlgorithmHttpClient> _logger;
        private readonly HttpClient _httpClient;
        private readonly List<AlgorithmService> _algorithmServices;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        /// <param name="httpClient"></param>
        public AlgorithmHttpClient(ILogger<AlgorithmHttpClient> logger, IOptions<List<AlgorithmService>> options, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _algorithmServices = options.Value;
        }

        #region 车牌识别

        /// <summary>
        /// 获取图像中全部的车牌对象(车牌识别)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <returns></returns>
        public async Task<ActionContent<List<PlateOcrContent>>> OcrPlatesAsync(string oprNum, string imgSource, ImageType imgType)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("车牌识别"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<车牌识别>算法服务信息");
                    return new ActionContent<List<PlateOcrContent>>(500, "未配置<车牌识别>算法服务信息");
                }

                var request = new PlateOcrRequest() { OprNum = oprNum, ImgType = imgType, ImgSource = imgSource };
                string json = JsonHelper.Serialize(request);

                _logger.LogDebug($"车牌识别请求：{json}");

                var httpResult = await _httpClient.SetBaseUri(algorithmService.Url).WithContent(json).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"车牌识别响应：{httpResult.Content}");

                    var result = JsonHelper.Deserialize<PlateOcrResponse>(httpResult.Content);
                    if (result.State == 0 && result.Plates.IsNotNullOrEmpty())
                    {
                        var response = new ActionContent<List<PlateOcrContent>>() { State = 200, Value = new List<PlateOcrContent>() };

                        foreach (var item in result.Plates)
                        {
                            response.Value.Add(new PlateOcrContent()
                            {
                                PlateNumber = item.PlateNumber,
                                PlateColor = item.PlateColor,
                                Score = item.Score,
                                Cover = item.Cover,
                                Points = item.Points
                            });

                            //
                            var content = await CorrectCarFaceAsync(oprNum, imgSource, imgType, item.Points);
                            if (content.State == 200)
                            {
                                var sss = await GetFaceModelAsync(oprNum, content.Value.FaceImgSource, ImageType.Base64, null);
                            }
                        }

                        return response;
                    }
                    else
                    {
                        if (result.State == 0)
                        {
                            return new ActionContent<List<PlateOcrContent>>(404);
                        }
                        else
                        {
                            return new ActionContent<List<PlateOcrContent>>(500, result.Desc);
                        }
                    }
                }
                else
                {
                    httpResult.Log(_logger, "车牌识别失败", json);
                    return new ActionContent<List<PlateOcrContent>>(500, httpResult.Exception.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "车牌识别失败");
                return new ActionContent<List<PlateOcrContent>>(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取图像中和目标车牌号码最匹配的对象(基于车牌识别)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <param name="plateNumber">目标车牌</param>
        /// <param name="minimumSimilarity">最小相似度</param>
        /// <returns></returns>
        public async Task<ActionContent<PlateOcrContent>> OcrPlateAsync(string oprNum, string imgSource, ImageType imgType, string plateNumber, double minimumSimilarity = 0.5)
        {
            try
            {
                var response = await OcrPlatesAsync(oprNum, imgSource, imgType);

                if (response.State != 200)
                {
                    //查询失败
                    return new ActionContent<PlateOcrContent>(response.State, response.Desc);
                }

                var plate = response.Value.FirstOrDefault(x => x.PlateNumber.Equals(plateNumber));
                if (plate == null)
                {
                    double highestSimilarity = 0;

                    foreach (var rect in response.Value)
                    {
                        double similarity = StringExtensions.Similarity(rect.PlateNumber, plateNumber);

                        if (similarity >= minimumSimilarity && similarity > highestSimilarity)
                        {
                            highestSimilarity = similarity;
                            plate = rect;
                        }
                    }
                }

                if (plate == null)
                {
                    return new ActionContent<PlateOcrContent>(404);
                }
                else
                {
                    return new ActionContent<PlateOcrContent>(200) { Value = plate };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "车牌识别失败");
                return new ActionContent<PlateOcrContent>(500, ex.Message);
            }
        }

        #endregion


        #region 九类检测

        /// <summary>
        /// 获取图像中全部的目标对象(九类检测)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <returns></returns>
        public async Task<ActionContent<List<MulticlassContent>>> DetectObjectsAsync(string oprNum, string imgSource, ImageType imgType)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("九类检测"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<九类检测>算法服务信息");
                    return new ActionContent<List<MulticlassContent>>(500, "未配置<九类检测>算法服务信息");
                }

                var request = new MulticlassRequest() { OprNum = oprNum, ImgType = imgType, ImgSource = imgSource };
                string json = JsonHelper.Serialize(request);

                _logger.LogDebug($"九类检测请求：{json}");

                var httpResult = await _httpClient.SetBaseUri(algorithmService.Url).WithContent(json).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"九类检测响应：{httpResult.Content}");

                    var result = JsonHelper.Deserialize<MulticlassResponse>(httpResult.Content);
                    if (result.State == 0 && result.Objects.IsNotNullOrEmpty())
                    {
                        var actionContent = new ActionContent<List<MulticlassContent>>() { State = 200, Value = new List<MulticlassContent>() };

                        foreach (var item in result.Objects)
                        {
                            actionContent.Value.Add(new MulticlassContent()
                            {
                                Label = item.Label,
                                Score = item.Score,
                                Points = new List<int>() { item.Left, item.Top, item.Right, item.Bottom }
                            });
                        }

                        return actionContent;
                    }
                    else
                    {
                        if (result.State == 0)
                        {
                            return new ActionContent<List<MulticlassContent>>(404);
                        }
                        else
                        {
                            return new ActionContent<List<MulticlassContent>>(500, result.Desc);
                        }
                    }
                }
                else
                {
                    httpResult.Log(_logger, "九类检测失败", json);
                    return new ActionContent<List<MulticlassContent>>(500, httpResult.Exception.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "九类检测失败");
                return new ActionContent<List<MulticlassContent>>(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取图像中指定类型的目标对象(基于九类检测)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <param name="labels">类型列表</param>
        /// <returns></returns>
        public async Task<ActionContent<List<MulticlassContent>>> DetectObjectsAsync(string oprNum, string imgSource, ImageType imgType, List<MulticlassType> labels)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("九类检测"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<九类检测>算法服务信息");
                    return new ActionContent<List<MulticlassContent>>(500, "未配置<九类检测>算法服务信息");
                }

                var request = new MulticlassRequest() { OprNum = oprNum, ImgType = imgType, ImgSource = imgSource };
                string json = JsonHelper.Serialize(request);

                _logger.LogDebug($"九类检测请求：{json}");

                var httpResult = await _httpClient.SetBaseUri(algorithmService.Url).WithContent(json).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"九类检测响应：{httpResult.Content}");

                    var result = JsonHelper.Deserialize<MulticlassResponse>(httpResult.Content);
                    if (result.State == 0 && result.Objects.IsNotNullOrEmpty())
                    {
                        var actionContent = new ActionContent<List<MulticlassContent>>() { State = 200, Desc = "", Value = new List<MulticlassContent>() };

                        foreach (var item in result.Objects)
                        {
                            if (labels.Contains(item.Label))
                            {
                                actionContent.Value.Add(new MulticlassContent()
                                {
                                    Label = item.Label,
                                    Score = item.Score,
                                    Points = new List<int>() { item.Left, item.Top, item.Right, item.Bottom }
                                });
                            }
                        }

                        return actionContent;
                    }
                    else
                    {
                        if (result.State == 0)
                        {
                            return new ActionContent<List<MulticlassContent>>(404);
                        }
                        else
                        {
                            return new ActionContent<List<MulticlassContent>>(500, result.Desc);
                        }
                    }
                }
                else
                {
                    httpResult.Log(_logger, "九类检测失败", json);
                    return new ActionContent<List<MulticlassContent>>(500, httpResult.Exception.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "九类检测失败");
                return new ActionContent<List<MulticlassContent>>(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取图像中和目标区域最匹配的对象(基于九类检测)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <param name="inpRect">目标区域</param>
        /// <returns></returns>
        public async Task<ActionContent<MulticlassContent>> DetectObjectAsync(string oprNum, string imgSource, ImageType imgType, Rectangle inpRect)
        {
            try
            {
                var response = await DetectObjectsAsync(oprNum, imgSource, imgType);

                if (response.State != 200)
                {
                    //查询失败
                    return new ActionContent<MulticlassContent>(response.State, response.Desc);
                }

                MulticlassContent maxIouObject = null;
                double maxIou = 0.0;

                double inpRectSize = inpRect.Width * inpRect.Height;

                foreach (var obj in response.Value)
                {
                    var rect = new Rectangle() { X = obj.Points[0], Y = obj.Points[1], Width = obj.Points[2] - obj.Points[0], Height = obj.Points[3] - obj.Points[1] };
                    double rectSize = rect.Width * rect.Height;

                    var intersectRect = Rectangle.Intersect(inpRect, rect);

                    double isize = intersectRect.Size.Width * intersectRect.Size.Height;
                    double usize = inpRectSize + rectSize - isize;

                    double iou = isize / usize;
                    if (iou > maxIou)
                    {
                        maxIou = iou;
                        maxIouObject = obj;
                    }
                }

                return new ActionContent<MulticlassContent>(200) { Value = maxIouObject };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "九类检测失败");
                return new ActionContent<MulticlassContent>(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取图像中指定类型的和目标区域最匹配的对象(基于九类检测)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <param name="labels">类型列表</param>
        /// <param name="inpRect">目标区域</param>
        /// <returns></returns>
        public async Task<ActionContent<MulticlassContent>> DetectObjectAsync(string oprNum, string imgSource, ImageType imgType, List<MulticlassType> labels, Rectangle inpRect)
        {
            try
            {
                var response = await DetectObjectsAsync(oprNum, imgSource, imgType, labels);

                if (response.State != 200)
                {
                    //查询失败
                    return new ActionContent<MulticlassContent>(response.State, response.Desc);
                }

                MulticlassContent maxIouObject = null;
                double maxIou = 0.0;

                double inpRectSize = inpRect.Width * inpRect.Height;

                foreach (var obj in response.Value)
                {
                    var rect = new Rectangle() { X = obj.Points[0], Y = obj.Points[1], Width = obj.Points[2] - obj.Points[0], Height = obj.Points[3] - obj.Points[1] };
                    double rectSize = rect.Width * rect.Height;

                    var intersectRect = Rectangle.Intersect(inpRect, rect);

                    double isize = intersectRect.Size.Width * intersectRect.Size.Height;
                    double usize = inpRectSize + rectSize - isize;

                    double iou = isize / usize;
                    if (iou > maxIou)
                    {
                        maxIou = iou;
                        maxIouObject = obj;
                    }
                }

                return new ActionContent<MulticlassContent>(200) { Value = maxIouObject };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "九类检测失败");
                return new ActionContent<MulticlassContent>(500, ex.Message);
            }
        }

        #endregion


        #region 车头车尾矫正

        /// <summary>
        /// 获取图像中矫正后的车头车尾对象
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <param name="platePoints">车牌顶点(x1,y1,x2,y2,x3,y3,x4,y4)</param>
        /// <returns></returns>
        public async Task<ActionContent<CarFaceCorrectContent>> CorrectCarFaceAsync(string oprNum, string imgSource, ImageType imgType, List<int> platePoints)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("车头车尾矫正"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<车头车尾矫正>算法服务信息");
                    return new ActionContent<CarFaceCorrectContent>(500, "未配置<车头车尾矫正>算法服务信息");
                }

                var request = new CarFaceCorrectRequest() { OprNum = oprNum, ImgType = imgType, ImgSource = imgSource, Points = platePoints };
                string json = JsonHelper.Serialize(request);

                _logger.LogDebug($"车头车尾矫正请求：{json}");

                var httpResult = await _httpClient.SetBaseUri(algorithmService.Url).WithContent(json).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"车头车尾矫正响应：{httpResult.Content}");

                    var result = JsonHelper.Deserialize<CarFaceCorrectResponse>(httpResult.Content);
                    if (result.State == 0)
                    {
                        return new ActionContent<CarFaceCorrectContent>(200)
                        {
                            Value = new CarFaceCorrectContent()
                            {
                                FaceType = result.Label == MulticlassType.head ? FaceType.车头 : FaceType.车尾,
                                FaceImgSource = result.FaceImgSource,
                                FacePoints = result.FacePoints
                            }
                        };
                    }
                    else
                    {
                        if (result.State == 0)
                        {
                            return new ActionContent<CarFaceCorrectContent>(404);
                        }
                        else
                        {
                            return new ActionContent<CarFaceCorrectContent>(500, result.Desc);
                        }
                    }
                }
                else
                {
                    httpResult.Log(_logger, "车头车尾矫正失败", json);
                    return new ActionContent<CarFaceCorrectContent>(500, httpResult.Exception.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "车头车尾矫正失败");
                return new ActionContent<CarFaceCorrectContent>(500, ex.Message);
            }
        }

        #endregion


        #region 车型识别

        /// <summary>
        /// 获取图像中车脸区域的车型识别结果(使用矫正的车脸图时，车脸区域应为<see cref="null"/>)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <param name="faceRect">车脸区域</param>
        /// <returns></returns>
        private async Task<ActionContent<CarModelSimpleContent>> GetFaceModelAsync(string oprNum, string imgSource, ImageType imgType, Rectangle? faceRect)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("车型识别"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<车型识别>算法服务信息");
                    return new ActionContent<CarModelSimpleContent>(500, "未配置<车型识别>算法服务信息");
                }

                var request = new CarModelRequest() { OprNum = oprNum, ImgType = imgType, ImgSource = imgSource };

                if (faceRect.HasValue)
                {
                    request.Left = faceRect.Value.Left;
                    request.Top = faceRect.Value.Top;
                    request.Right = faceRect.Value.Right;
                    request.Bottom = faceRect.Value.Bottom;
                }

                string json = JsonHelper.Serialize(request);

                _logger.LogDebug($"车型识别请求：{json}");

                var httpResult = await _httpClient.SetBaseUri(algorithmService.Url).WithContent(json).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"车型识别响应：{httpResult.Content}");

                    var result = JsonHelper.Deserialize<CarModelResponse>(httpResult.Content);
                    if (result.State == 0)
                    {
                        return new ActionContent<CarModelSimpleContent>(200)
                        {
                            Value = new CarModelSimpleContent()
                            {
                                Label = result.Label,
                                Name = result.Name,
                                Confidence = result.Confidence
                            }
                        };
                    }
                    else
                    {
                        return new ActionContent<CarModelSimpleContent>(500, result.Desc);
                    }
                }
                else
                {
                    httpResult.Log(_logger, "车型识别失败", json);
                    return new ActionContent<CarModelSimpleContent>(500, httpResult.Exception.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "车型识别失败");
                return new ActionContent<CarModelSimpleContent>(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取图像中全部的车脸区域车型识别结果(基于九类检测)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <returns></returns>
        public async Task<ActionContent<List<CarModelExtendContent>>> DetectModelsAsync(string oprNum, string imgSource, ImageType imgType)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("车型识别"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<车型识别>算法服务信息");
                    return new ActionContent<List<CarModelExtendContent>>(500, "未配置<车型识别>算法服务信息");
                }

                var response = await DetectObjectsAsync(oprNum, imgSource, imgType, new List<MulticlassType>() { MulticlassType.head, MulticlassType.tail });

                if (response.State != 200)
                {
                    //查询失败
                    return new ActionContent<List<CarModelExtendContent>>(response.State, response.Desc);
                }

                var actionContent = new ActionContent<List<CarModelExtendContent>>() { State = 200, Desc = "", Value = new List<CarModelExtendContent>() };

                foreach (var item in response.Value)
                {
                    var result = await GetFaceModelAsync(Guid.NewGuid().ToString(), imgSource, imgType, new Rectangle(item.Points[0], item.Points[1], item.Points[2] - item.Points[0], item.Points[3] - item.Points[1]));

                    if (result.State == 200)
                    {
                        var content = new CarModelExtendContent()
                        {
                            FaceType = item.Label == MulticlassType.head ? FaceType.车头 : FaceType.车尾,
                            Label = result.Value.Label,
                            Name = result.Value.Name,
                            Confidence = result.Value.Confidence,
                            Points = item.Points
                        };

                        actionContent.Value.Add(content);
                    }


                }

                return actionContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "车型识别失败");
                return new ActionContent<List<CarModelExtendContent>>(500, ex.Message);
            }
        }

        /// <summary>
        /// 获取图像中和目标区域最匹配的车脸车型识别结果(基于九类检测)
        /// </summary>
        /// <param name="oprNum"></param>
        /// <param name="imgSource"></param>
        /// <param name="imgType"></param>
        /// <param name="inpRect">目标区域</param>
        /// <returns></returns>
        public async Task<ActionContent<CarModelExtendContent>> DetectModelAsync(string oprNum, string imgSource, ImageType imgType, Rectangle inpRect)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("车型识别"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<车型识别>算法服务信息");
                    return new ActionContent<CarModelExtendContent>(500, "未配置<车型识别>算法服务信息");
                }

                var response = await DetectObjectAsync(oprNum, imgSource, imgType, new List<MulticlassType>() { MulticlassType.head, MulticlassType.tail }, inpRect);

                if (response.State != 200)
                {
                    //查询失败
                    return new ActionContent<CarModelExtendContent>(response.State, response.Desc);
                }

                var result = await GetFaceModelAsync(Guid.NewGuid().ToString(), imgSource, imgType, new Rectangle(response.Value.Points[0], response.Value.Points[1], response.Value.Points[2] - response.Value.Points[0], response.Value.Points[3] - response.Value.Points[1]));

                if (result.State == 200)
                {
                    return new ActionContent<CarModelExtendContent>(200)
                    {
                        Value = new CarModelExtendContent()
                        {
                            FaceType = response.Value.Label == MulticlassType.head ? FaceType.车头 : FaceType.车尾,
                            Label = result.Value.Label,
                            Name = result.Value.Name,
                            Confidence = result.Value.Confidence,
                            Points = response.Value.Points
                        }
                    };
                }
                else
                {
                    return new ActionContent<CarModelExtendContent>(500, result.Desc);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "车型识别失败");
                return new ActionContent<CarModelExtendContent>(500, ex.Message);
            }
        }

        #endregion


        #region 泊位高点映射

        /// <summary>
        /// 泊位画线高点映射
        /// </summary>
        /// <param name="cameraWidth"></param>
        /// <param name="cameraHeight"></param>
        /// <param name="berthLength"></param>
        /// <param name="berthWidth"></param>
        /// <param name="berthHeight"></param>
        /// <param name="lowRegionPoints">低点数据</param>
        /// <returns></returns>
        public async Task<BerthRegionReshapeResponse> BerthRegionReshapeAsync(int cameraWidth, int cameraHeight, double berthLength, double berthWidth, double berthHeight, List<Point> lowRegionPoints)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("泊位高点映射"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<泊位高点映射>算法服务信息");
                    return null;
                }

                //参数校验
                if (cameraWidth < 1 || cameraHeight < 1)
                    return null;

                var request = new BerthRegionReshapeRequest()
                {
                    OprNum = Guid.NewGuid().ToString(),
                    CameraWidth = cameraWidth,
                    CameraHeight = cameraHeight,
                    BerthLength = berthLength,
                    BerthWidth = berthWidth,
                    BerthHeight = berthHeight,
                    BerthPoints = new List<double>()
                };

                foreach (var point in lowRegionPoints)
                {
                    //归一化
                    request.BerthPoints.Add(point.X * 1.0 / cameraWidth);
                    request.BerthPoints.Add(point.Y * 1.0 / cameraHeight);
                }

                string json = JsonHelper.Serialize(request);
                _logger.LogDebug($"泊位高点映射请求：{json}");

                var httpResult = await _httpClient.SetBaseUri(algorithmService.Url).WithContent(json).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"泊位高点映射响应：{httpResult.Content}");

                    var result = JsonHelper.Deserialize<BerthRegionReshapeResponse>(httpResult.Content);
                    if (result != null && result.State == 0 && result.BerthPoints != null && result.BerthPoints.Count == 16)
                    {
                        return result;
                    }
                    else
                    {
                        _logger.LogDebug($"泊位高点映射失败：{httpResult.Content}");
                        return null;
                    }
                }
                else
                {
                    httpResult.Log(_logger, "泊位高点映射失败", json);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "泊位高点映射失败");
            }

            return null;
        }

        #endregion


        #region 泊位历史状态

        /// <summary>
        /// 泊位历史状态查询
        /// </summary>
        /// <param name="parkCode"></param>
        /// <param name="berthCode"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<ActionContent<BerthHistoryStateContent>> BerthHistoryStatesAsync(string parkCode, string berthCode, DateTime beginTime, DateTime endTime)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("泊位历史状态"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<泊位历史状态>算法服务信息");
                    return new ActionContent<BerthHistoryStateContent>(500, "未配置<泊位历史状态>算法服务信息");
                }

                return await BerthHistoryStatesAsync(algorithmService.Url, string.Empty, parkCode, berthCode, beginTime, endTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "泊位历史状态查询失败");
                return new ActionContent<BerthHistoryStateContent>(500, ex.Message);
            }
        }

        /// <summary>
        /// 泊位历史状态查询
        /// </summary>
        /// <param name="serviceUrl"></param>
        /// <param name="serviceToken"></param>
        /// <param name="parkCode"></param>
        /// <param name="berthCode"></param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<ActionContent<BerthHistoryStateContent>> BerthHistoryStatesAsync(string serviceUrl, string serviceToken, string parkCode, string berthCode, DateTime beginTime, DateTime endTime)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serviceUrl))
                {
                    _logger.LogError("<泊位历史状态>服务地址为空");
                    return new ActionContent<BerthHistoryStateContent>(500, "<泊位历史状态>服务地址为空");
                }

                var request = new BerthHistoryStateRequest() { ParkCode = parkCode, BerthCode = berthCode, BeginTime = beginTime, EndTime = endTime };
                string json = JsonHelper.Serialize(request);

                _logger.LogDebug($"泊位历史状态查询请求：{json}");

                var headers = string.IsNullOrWhiteSpace(serviceToken) ? default : new Dictionary<string, object>() { { "Authorization", serviceToken } };

                var httpResult = await _httpClient.SetBaseUri(serviceUrl).WithHeaders(headers).WithContent(json).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"泊位历史状态查询响应：{parkCode},{berthCode},{httpResult.Content}");

                    var result = JsonHelper.Deserialize<BerthHistoryStateResponse>(httpResult.Content);
                    if (result.State == 0 && result.HistoryStates.IsNotNullOrEmpty())
                    {
                        var response = new BerthHistoryStateContent()
                        {
                            ParkCode = result.ParkCode,
                            BerthCode = result.BerthCode,
                            BeginTime = result.BeginTime,
                            EndTime = result.EndTime,
                            HistoryStates = new List<BerthHistoryStateItem>()
                        };

                        foreach (var item in result.HistoryStates.OrderBy(x => x.Timestamp))
                        {
                            response.HistoryStates.Add(new BerthHistoryStateItem()
                            {
                                ParkState = item.ParkState,
                                Timestamp = DateTimeOffset.FromUnixTimeSeconds(item.Timestamp).LocalDateTime
                            });
                        }

                        return new ActionContent<BerthHistoryStateContent>(200) { Value = response };
                    }
                    else
                    {
                        if (result.State == 0)
                        {
                            return new ActionContent<BerthHistoryStateContent>(404);
                        }
                        else
                        {
                            return new ActionContent<BerthHistoryStateContent>(500, result.Desc);
                        }
                    }
                }
                else
                {
                    httpResult.Log(_logger, "泊位历史状态查询失败", json);
                    return new ActionContent<BerthHistoryStateContent>(500, httpResult.Exception.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "泊位历史状态查询失败");
                return new ActionContent<BerthHistoryStateContent>(500, ex.Message);
            }
        }

        #endregion


        #region 设备按需取图

        /// <summary>
        /// 按需取图
        /// </summary>
        /// <param name="parkCode">车场编码</param>
        /// <param name="berthCode">泊位编码</param>
        /// <param name="dateTime">时间戳(秒)</param>
        /// <param name="oprNum">流水号</param>
        /// <returns></returns>
        public async Task<ActionContent<string>> GetDeviceImageAsync(string oprNum, string parkCode, string berthCode, DateTime dateTime)
        {
            try
            {
                var algorithmService = _algorithmServices.FirstOrDefault(x => x.Name.Equals("按需取图"));
                if (algorithmService == null)
                {
                    _logger.LogError("未配置<按需取图>蓝脑服务信息");
                    return new ActionContent<string>(500, "未配置<按需取图>蓝脑服务信息");
                }

                //发起取图请求
                var firstRequest = new DeviceFirstRequest() { ParkCode = parkCode, BerthCode = berthCode, Type = 1, StartTime = dateTime.ToUnixTimeSeconds(), Tag = "alg" };

                string firstRequestJson = JsonHelper.Serialize(firstRequest);
                _logger.LogDebug($"按需取图注册请求：{oprNum}，{firstRequestJson}");

                var httpResult = await _httpClient.SetBaseUri($"{algorithmService.Url}bd/gf").WithContent(firstRequestJson).PostAsync();
                if (httpResult.IsSuccess)
                {
                    _logger.LogDebug($"按需取图注册响应：{oprNum}，{httpResult.Content}");

                    var firstResult = JsonHelper.Deserialize<DeviceFirstRequestResult>(httpResult.Content);
                    if (firstResult != null)
                    {
                        if (firstResult.Code == 1)
                        {
                            await Task.Delay(500);
                            string taskId = firstResult.TaskId;

                            //
                            int maxTimes = 100;
                            int times = 0;

                            while (times < maxTimes)
                            {
                                httpResult = await _httpClient.SetBaseUri($"{algorithmService.Url}bd/gf/image/{taskId}").PostAsync();
                                if (httpResult.IsSuccess)
                                {
                                    _logger.LogDebug($"按需取图查询响应：{oprNum}，{httpResult.Content}");

                                    var secondResult = JsonHelper.Deserialize<DeviceSecondRequestResult>(httpResult.Content);
                                    if (secondResult != null)
                                    {
                                        switch (secondResult.Code)
                                        {
                                            case 1:
                                                //成功
                                                if (secondResult.Value != null)
                                                {
                                                    _logger.LogDebug($"按需取图查询成功：{oprNum}，{httpResult.Content}");
                                                    return new ActionContent<string>(200) { Value = secondResult.Value.ImgUrl.Replace("-internal", "") };
                                                }
                                                else
                                                {
                                                    times = maxTimes;
                                                    _logger.LogError($"按需取图解析失败：{oprNum}，{httpResult.Content}");
                                                }
                                                break;
                                            case -1:
                                                //继续查询
                                                await Task.Delay(100);
                                                times++;
                                                break;
                                            default:
                                                //失败
                                                times = maxTimes;
                                                _logger.LogError($"按需取图查询失败：{oprNum}，{httpResult.Content}");
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        times = maxTimes;
                                        _logger.LogError($"按需取图解析失败：{oprNum}，{httpResult.Content}");
                                    }
                                }
                                else
                                {
                                    _logger.LogError(httpResult.Exception, $"按需取图查询异常：{oprNum}");
                                    times = maxTimes;
                                }
                            }
                        }
                        else
                        {
                            _logger.LogError($"按需取图注册失败：{oprNum}，{httpResult.Content}");
                        }
                    }
                    else
                    {
                        _logger.LogError($"按需取图解析异常：{oprNum}，{httpResult.Content}");
                    }
                }
                else
                {
                    _logger.LogError(httpResult.Exception, $"按需取图请求异常：{oprNum}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "按需取图异常");
            }

            return new ActionContent<string>(500, "按需取图失败");
        }

        #endregion
    }
}
