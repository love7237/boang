using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using T.Test.WebApi.Models;
using T.Utility.OSS;
using T.Utility.Protocol;

namespace T.Test.WebApi.Controllers
{
    /// <summary>
    /// 对象存储控制器
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class OssController : ControllerBase
    {
        private readonly ILogger<OssController> _logger;
        private readonly OssHelper _ossHelper;

        public OssController(ILogger<OssController> logger, OssHelper ossHelper)
        {
            _logger = logger;
            _ossHelper = ossHelper;
        }

        /// <summary>
        /// 检查bucket是否存在
        /// </summary>
        /// <returns></returns>
        [HttpGet("bucket/exist")]
        public async Task<ActionContent<bool>> DoesBucketExist()
        {
            return await _ossHelper.DoesBucketExist();
        }

        /// <summary>
        /// 检查object是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("object/exist")]
        public async Task<ActionContent<bool>> DoesObjectExist(string key)
        {
            return await _ossHelper.DoesObjectExist(key);
        }

        /// <summary>
        /// 下载object文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet("object/get")]
        public async Task<ActionResult> GetObject(string key)
        {
            var result = await _ossHelper.GetObject(key);
            if (result.State == 200 && result.Value != null)
            {
                return File(result.Value, "application/octet-stream", key.Split('/').Last());
            }
            else
            {
                return BadRequest(result.Desc);
            }
        }

        /// <summary>
        /// 上传object文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("object/put")]
        public async Task<ActionContent<string>> PutObject(AlgorithmRequest request)
        {
            byte[] bytes = null;

            if (request.ImageType == ImageType.Url)
            {
                var result = await _ossHelper.ProxyDownload(request.ImageSource);
                if (result.State == 200 && result.Value != null)
                {
                    bytes = result.Value;
                }
            }
            else
            {
                bytes = Convert.FromBase64String(request.ImageSource);
            }

            if (bytes != null)
            {
                string key = $"{DateTime.Now:yyyyMMdd}/{Guid.NewGuid()}.jpg";

                var result = await _ossHelper.PutObject(key, new MemoryStream(bytes));

                if (result.State == 200 && _ossHelper.TryDecodeUrl(result.Value, out string url))
                {
                    result.Value = url;
                }

                return result;
            }
            else
            {
                return new ActionContent<string>() { State = 400, Desc = "图像上传失败" };
            }
        }

        /// <summary>
        /// 删除object文件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("object/delete")]
        public async Task<ActionContent<bool>> DeleteObject(string key)
        {
            return await _ossHelper.DeleteObject(key);
        }

        /// <summary>
        /// 代理下载
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("proxy/download")]
        public async Task<ActionResult> ProxyDownload(AlgorithmRequest request)
        {
            byte[] bytes = null;

            if (request.ImageType == ImageType.Url)
            {
                var result = await _ossHelper.ProxyDownload(request.ImageSource);
                if (result.State == 200 && result.Value != null)
                {
                    bytes = result.Value;
                }
            }
            else
            {
                bytes = Convert.FromBase64String(request.ImageSource);
            }

            if (bytes != null)
            {
                return File(bytes, "application/octet-stream", request.ImageSource.Split('/').Last());
            }
            else
            {
                return BadRequest("代理下载失败");
            }
        }

        /// <summary>
        /// 文件对象存储流下载
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("/file/{fileName}")]
        public ActionResult FileStorage(string fileName)
        {
            return FileStorage(new List<string>() { fileName });
        }

        /// <summary>
        /// 文件对象存储流下载
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("/file/{path1}/{fileName}")]
        public ActionResult FileStorage(string path1, string fileName)
        {
            return FileStorage(new List<string>() { path1, fileName });
        }

        /// <summary>
        /// 文件对象存储流下载
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("/file/{path1}/{path2}/{fileName}")]
        public ActionResult FileStorage(string path1, string path2, string fileName)
        {
            return FileStorage(new List<string>() { path1, path2, fileName });
        }

        /// <summary>
        /// 文件对象存储流下载
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("/file/{path1}/{path2}/{path3}/{fileName}")]
        public ActionResult FileStorage(string path1, string path2, string path3, string fileName)
        {
            return FileStorage(new List<string>() { path1, path2, path3, fileName });
        }

        /// <summary>
        /// 文件对象存储流下载
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <param name="path4"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("/file/{path1}/{path2}/{path3}/{path4}/{fileName}")]
        public ActionResult FileStorage(string path1, string path2, string path3, string path4, string fileName)
        {
            return FileStorage(new List<string>() { path1, path2, path3, path4, fileName });
        }

        /// <summary>
        /// 文件对象存储流下载
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <param name="path3"></param>
        /// <param name="path4"></param>
        /// <param name="path5"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet("/file/{path1}/{path2}/{path3}/{path4}/{path5}/{fileName}")]
        public ActionResult FileStorage(string path1, string path2, string path3, string path4, string path5, string fileName)
        {
            return FileStorage(new List<string>() { path1, path2, path3, path4, path5, fileName });
        }

        /// <summary>
        /// 文件对象存储流下载
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        [NonAction]
        private ActionResult FileStorage(List<string> paths)
        {
            string key = string.Join('/', paths);

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "oss", key);

            if (System.IO.File.Exists(path))
            {
                var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                return File(fileStream, "application/octet-stream", paths.Last());
            }
            else
            {
                return NotFound("The specified key does not exist.");
            }
        }

    }
}
