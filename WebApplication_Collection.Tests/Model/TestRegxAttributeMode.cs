using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication_Collection.Models.Attribute.Regex;
using WebApplicationV.Utils.Attribute;

namespace WebApplication_Collection.Tests.Model
{
    public class TestRegxAttributeMode
    {
        [RegularExpressionDictionary(EnumDictionaryDescription.Regex_Null)]
        public string Parameter_string { get; set; }
        [RegularExpressionDictionary(EnumDictionaryDescription.Regex_Password)]
        public string Parameter_password { get; set; }
    }
}
