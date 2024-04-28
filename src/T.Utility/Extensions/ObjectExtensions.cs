using System;
using System.Reflection;

namespace T.Utility.Extensions
{
    /// <summary>
    /// Object extension methods for common scenarios.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 以深拷贝的方式创建对象的副本
        /// </summary>
        /// <param name="srcObject"></param>
        /// <returns></returns>
        public static object Copy(this object srcObject)
        {
            if (srcObject == null)
                return null;

            object destObject;

            Type srcType = srcObject.GetType();
            if (srcType.IsValueType)
            {
                //值类型：直接复制
                destObject = srcObject;
            }
            //引用类型   
            else
            {
                //引用类型：通过反射的方式复制
                destObject = Activator.CreateInstance(srcType);   //创建引用对象   
                MemberInfo[] memberInfos = srcObject.GetType().GetMembers();

                foreach (MemberInfo memberInfo in memberInfos)
                {
                    if (memberInfo.MemberType == MemberTypes.Field)
                    {
                        //字段
                        FieldInfo field = (FieldInfo)memberInfo;
                        object fieldValue = field.GetValue(srcObject);
                        if (fieldValue is ICloneable)
                        {
                            field.SetValue(destObject, (fieldValue as ICloneable).Clone());
                        }
                        else
                        {
                            field.SetValue(destObject, Copy(fieldValue));
                        }
                    }
                    else if (memberInfo.MemberType == MemberTypes.Property)
                    {
                        //属性
                        PropertyInfo property = (PropertyInfo)memberInfo;
                        MethodInfo info = property.GetSetMethod(false);
                        if (info != null)
                        {
                            object propertyValue = property.GetValue(srcObject, null);
                            if (propertyValue is ICloneable)
                            {
                                property.SetValue(destObject, (propertyValue as ICloneable).Clone(), null);
                            }
                            else
                            {
                                property.SetValue(destObject, Copy(propertyValue), null);
                            }
                        }
                    }
                }
            }
            return destObject;
        }
    }
}
