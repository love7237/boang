using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace T.Utility.Extensions
{
    /// <summary>
    /// Validation extension methods for common scenarios.
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Validates the specified object.
        /// </summary>
        /// <param name="obj">The object to validate.</param>
        /// <param name="error">The error message to associate with a validation control.</param>
        /// <returns></returns>
        public static bool TryValidate(this object obj, out string error)
        {
            if (obj == null || obj.GetType().IsValueType)
            {
                error = "object is null or value type";
                return false;
            }

            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.IsDefined(typeof(ValidationAttribute), false))
                {
                    if (property.GetCustomAttributes(typeof(ValidationAttribute), false) is ValidationAttribute[] validationAttributes)
                    {
                        foreach (ValidationAttribute validationAttribute in validationAttributes)
                        {
                            object value = property.GetValue(obj, null);
                            if (validationAttribute.IsValid(value))
                            {
                                continue;
                            }
                            else
                            {
                                if (property.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] descriptionAttributes && descriptionAttributes.Length > 0)
                                {
                                    error = validationAttribute.FormatErrorMessage(descriptionAttributes[0].Description);
                                    return false;
                                }
                                else
                                {
                                    error = validationAttribute.FormatErrorMessage(property.Name);
                                    return false;
                                }
                            }
                        }
                    }
                }
            }

            error = string.Empty;
            return true;
        }
    }
}
