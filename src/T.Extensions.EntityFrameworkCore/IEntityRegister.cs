using Microsoft.EntityFrameworkCore;

namespace T.Extensions.EntityFrameworkCore
{
    /// <summary>
    /// 数据库实体注册接口
    /// </summary>
    public interface IEntityRegister
    {
        /// <summary>
        /// 将当前实体类映射对象注册到数据上下文模型构建器中
        /// </summary>
        /// <param name="modelBuilder">上下文模型构建器</param>
        void RegistTo(ModelBuilder modelBuilder);
    }

    /// <summary>
    /// 数据库实体注册接口
    /// </summary>
    /// <typeparam name="TEntity">实体类型</typeparam>
    public interface IEntityRegister<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        //
    }
}
