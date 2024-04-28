using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace T.Extensions.EntityFrameworkCore
{
    /// <summary>
    /// 数据库实体注册接口辅助类
    /// </summary>
    public partial class IEntityRegisterHelper
    {
        /// <summary>
        /// 返回数据库实体注册接口实例集合
        /// </summary>
        /// <param name="path">文件目录</param>
        /// <param name="searchPattern">包含通配符(*和?)的程序集文件搜索字符串</param>
        /// <returns></returns>
        public static IEnumerable<IEntityRegister> FindRegisters(string path, string searchPattern = "*.dll")
        {
            var files = Directory.GetFiles(path, searchPattern);
            foreach (var file in files)
            {
                Assembly assembly = Assembly.LoadFrom(file);
                foreach (var x in assembly.DefinedTypes)
                {
                    if (x.IsClass && !x.IsAbstract && typeof(IEntityRegister).IsAssignableFrom(x))
                    {
                        IEntityRegister entityRegister = Activator.CreateInstance(x) as IEntityRegister;
                        yield return entityRegister;
                    }
                }
            }
        }
    }

}
