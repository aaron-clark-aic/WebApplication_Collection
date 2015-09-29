using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication_Collection.Models.Attribute.Regex;
using WebApplicationV.Utils.Attribute;

namespace WebApplication_Collection.Tests.Model
{
    /// <summary>
    /// 用于测试使用enum提取说明来验证正则表达式的测试类
    /// [aaron_clark_aic][20150929][create]
    /// </summary>
    public class TestRegxAttributeMode
    {
        [RegularExpressionDictionary(EnumDictionaryDescription.Regex_Null)]
        public string Parameter_string { get; set; }
        [RegularExpressionDictionary(EnumDictionaryDescription.Regex_Password)]
        public string Parameter_password { get; set; }
    }
}
