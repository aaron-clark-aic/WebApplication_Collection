using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace WebApplication_Collection.Models.Attribute.Regex
{
    /// <summary>
    /// 获取enum的string描述,用于这个描述到可以使用的regex验证中当做正则的字符串
    /// [aaron_clark_aic][20150928][create]
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
     public sealed class EnumDescriptionAttribute : FilterAttribute
    {
        private string description;
        public string Description { get { return description; } }

        public EnumDescriptionAttribute(string description)
            : base()
        {
            this.description = description;
        }
    }

    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            if (value == null)
            {
                throw new ArgumentException("value");
            }
            string description = value.ToString();
            var fieldInfo = value.GetType().GetField(description);
            var attributes =
                (EnumDescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                description = attributes[0].Description;
            }
            return description;
        }
    }
    //demo
    //Console.WriteLine(EnuHelper.GetDescription(EnumDictionaryDescription.Regex_Null))  
}