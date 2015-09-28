using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication_Collection.Models.Attribute.Regex
{
    public enum EnumDictionaryDescription
    {
        /// <summary>
        /// 空
        /// </summary>
        [EnumDescription("N")]
        Regex_Null,
        /// <summary>
        /// 电话号码验证
        /// </summary>
        [EnumDescription("^1[3|4|5|8]{1}[0-9]{9}$")]
        Regex_TelePhone,
        /// <summary>
        /// 验证只能是正整数加0
        /// </summary>
        [EnumDescription("^[0-9]*$")]
        Regex_Number,
        /// <summary>
        /// 验证只能为正整数
        /// </summary>
        [EnumDescription("^[0-9]*[1-9][0-9]*$")]
        Regex_PositiveNumber,
        /// <summary>
        /// 验证正浮点数
        /// </summary>
        [EnumDescription(@"^\d+\.?\d*$")]
        Regex_PositiveFloat,
        /// <summary>
        /// 验证车牌
        /// </summary>
        [EnumDescription("^[\u4e00-\u9fa5]{1}[A-Z]{1}[A-Z_0-9]{5}$")]
        Regex_LicensePlate,
        /// <summary>
        /// 验证密码
        /// </summary>
        [EnumDescription(@"[\w]{8,}")]
        Regex_Password,
        /// <summary>
        /// 字母数字下划线汉字
        /// </summary>
        [EnumDescription("^[a-zA-Z0-9_\u4e00-\u9fa5]+$")]
        Regex_IllegalCharacter,
        /// <summary>
        /// 图片后缀
        /// </summary>
        [EnumDescription (".(?i:(jpg)|(gif)|(bmp)|(png)|(jpeg))$")]
        Regex_Image,
        /// <summary>
        /// 0/1  bool
        /// </summary>
        [EnumDescription ("^[0|1]$")]
        Regex_ShortNumber,
        /// <summary>
        /// 验证车架号 17位字母数字组合
        /// </summary>
        [EnumDescription ("^[A-Za-z0-9]{17}$")]
        Regex_ChassisNumber,
    }
}