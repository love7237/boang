using System.IO;
using System.Threading.Tasks;
using T.Utility.Protocol;

namespace T.Utility.OSS
{
    /// <summary>
    /// 对象存储适配
    /// </summary>
    public interface IAdapter
    {
        /// <summary>
        /// Checks if the bucket exists
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns>True when the bucket exists under the current user;Otherwise returns false.</returns>
        public Task<ActionContent<bool>> DoesBucketExist(string bucket);

        /// <summary>
        /// Creates a new bucket
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns>True when success;Otherwise returns false.</returns>
        public Task<ActionContent<bool>> CreateBucket(string bucket);

        /// <summary>
        /// Checks if the object exists
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns>True when the object exists;Otherwise returns false.</returns>
        public Task<ActionContent<bool>> DoesObjectExist(string bucket, string key);

        /// <summary>
        /// Gets object
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns>Returns the byte array when success;Otherwise returns null.</returns>
        public Task<ActionContent<byte[]>> GetObject(string bucket, string key);

        /// <summary>
        /// Puts object to the specified bucket with specified object key
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <param name="content"></param>
        /// <returns>Returns the url string when success;Otherwise returns null.</returns>
        public Task<ActionContent<string>> PutObject(string bucket, string key, Stream content);

        /// <summary>
        /// Deletes object
        /// </summary>
        /// <param name="bucket"></param>
        /// <param name="key"></param>
        /// <returns>True when success;Otherwise returns false.</returns>
        public Task<ActionContent<bool>> DeleteObject(string bucket, string key);
    }
}
