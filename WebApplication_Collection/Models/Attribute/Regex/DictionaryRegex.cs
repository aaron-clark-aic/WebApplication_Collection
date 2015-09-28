using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplicationV.Utils.Attribute
{
    public class DictionaryRegex
    {
        //public static string Regex_TelePhone = "(d+-)?(d{4}-?d{7}|d{3}-?d{8}|^d{7,8})(-d+)?";
        public static Regex Regex(string str) {
             string return_str = null;
            switch (str)
            {
                case "Regex_Null":   
                    return_str = "N";
                    break;
                case "Regex_TelePhone":      //电话号码验证
                    return_str= "^1[3|4|5|8]{1}[0-9]{9}$";
                    break;
                case "Regex_Number":        //验证只能是正整数加0
                    return_str = "^[0-9]*$";
                    break;
                case "Regex_PositiveNumber":                //验证只能为正整数
                    return_str = "^[0-9]*[1-9][0-9]*$";
                    break;
                case "Regex_PositiveFloat":                  //验证正浮点数
                    return_str = @"^\d+\.?\d*$";
                    //return_str = @"^(([0-9]+\.[0-9]*[0-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9][1-9]*[0-9]*))$";
                    break;
                case "Regex_LicensePlate":                   //验证车牌
                    return_str = "^[\u4e00-\u9fa5]{1}[A-Z]{1}[A-Z_0-9]{5}$";
                    break;
                case "Regex_Password":                      //  验证密码
                    return_str = @"[\w]{8,}";
                    break;
                case "Regex_IllegalCharacter":           //字母数字下划线汉字
                    return_str = "^[a-zA-Z0-9_\u4e00-\u9fa5]+$";
                    break;
                case "Regex_Image":
                    //return_str = "(?i).+?\\.(jpg|gif|bmp|png|jpeg)";
                    return_str = ".(?i:(jpg)|(gif)|(bmp)|(png)|(jpeg))$";
                    break;
                case "Regex_ShortNumber":     //只能匹配0和1
                    return_str = "^[0|1]$";
                    break;
                case "Regex_ChassisNumber":
                    return_str = "^[A-Za-z0-9]{17}$";  //验证车架号 17位字母数字组合
                    break;
                default:
                    return_str = "";
                    break;
            }
            return new Regex(return_str,RegexOptions.Compiled);
        }
    }
}