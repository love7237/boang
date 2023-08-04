using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace T.Utility.Protocol
{
    /// <summary>
    /// 分页查询参数
    /// </summary>
    [DataContract]
    public class PaginationParameters
    {
        /// <summary>
        /// 页码
        /// </summary>
        [Range(1, int.MaxValue)]
        [DataMember(Name = "pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [Range(1, 500)]
        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// 按条件倒序查询
        /// </summary>
        [DataMember(Name = "descending")]
        public bool Descending { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationParameters"/> class.
        /// </summary>
        public PaginationParameters() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationParameters"/> struct with the specified parameters.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PaginationParameters(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Descending = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationParameters"/> struct with the specified parameters.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="descending"></param>
        public PaginationParameters(int pageNumber, int pageSize, bool descending)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Descending = descending;
        }
    }

    /// <summary>
    /// 分页查询结果
    /// </summary>
    [DataContract]
    public class PaginationResult<T>
    {
        /// <summary>
        /// 页码
        /// </summary>
        [DataMember(Name = "pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// 记录总数
        /// </summary>
        [DataMember(Name = "totalItems")]
        public int TotalItems { get; set; }

        /// <summary>
        /// 分页数据集合
        /// </summary>
        [DataMember(Name = "pageItems")]
        public ICollection<T> PageItems { get; set; }

        /// <summary>
        /// Initializes a new instance of the PaginationResult class.
        /// </summary>
        public PaginationResult() { }

        /// <summary>
        /// Initializes a new instance of the PaginationResult class with the specified parameters.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public PaginationResult(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
