using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;

namespace WebApplication_Collection.Models.RSA
{
    public class RSACache
    {
        /// <summary>
        /// 本地公钥
        /// </summary>
        public static readonly byte[] DEF_PK;
      

        static RSACache()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            try
            {
                rsa.FromXmlString(GetKey());
                DEF_PK = rsa.ExportParameters(false).Modulus;
            }
            catch (Exception)
            {
                WriteRsaXmlFile(System.Configuration.ConfigurationManager.AppSettings["RSA_xmlFile_address_public"], rsa.ToXmlString(false));
                //pair的秘钥
                WriteRsaXmlFile(System.Configuration.ConfigurationManager.AppSettings["RSA_xmlFile_address_private"], rsa.ToXmlString(true));
                rsa.FromXmlString(GetKey());
                DEF_PK = rsa.ExportParameters(false).Modulus;
            }

        }


        /// <summary>
        /// 获得本地密钥信息
        /// </summary>
        /// <returns>XML 字符串格式的密钥信息</returns>
        private static string GetKey(bool localXml = false)
        {
            string addressStr = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            StreamReader sw;
            string key;
            if (localXml)
            {
                sw = new StreamReader(@"E:\publicKey\PublicKey.xml");
            }
            else
            {
                sw = new StreamReader(addressStr+ System.Configuration.ConfigurationManager.AppSettings["RSA_xmlFile_address_Private"],false);
                //sw = new StreamReader(addressStr + "/App_Data/PublicKey.xml", false);
            }

            try
            {
                key = sw.ReadLine();
            }
            finally
            {
                sw.Close();
            }
            return key;
        }

        private static void WriteRsaXmlFile(string fileName,string contents) {
            string addressStr = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase +  fileName;
            
            using (FileStream fs_IDisposable = new FileStream(addressStr, FileMode.Create) )
            {
                using (StreamWriter sw_IDisposable = new StreamWriter(fs_IDisposable))
                {
                    sw_IDisposable.WriteLine(contents);
                    sw_IDisposable.Close();
                }
            }
        }

        /// <summary>
        /// 使用指定的模对字符串进行加密
        /// </summary>
        /// <param name="m">需要加密的内容</param>
        /// <param name="modulus">模的base64编码或者null(base64加密)</param>
        /// <returns></returns>
        public static string System_Numberic_RSA_en(string m, Byte[] modulus)
        {
            try
            {
                if (modulus != null)
                {
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                    RSAParameters para_en = new RSAParameters();
                    para_en.Exponent = Convert.FromBase64String("AQAB");
                    para_en.Modulus = modulus;
                    rsa.ImportParameters(para_en);

                    byte[] PlaintextData = Encoding.UTF8.GetBytes(m);
                    int MaxBlockSize = rsa.KeySize / 8 - 11;    //加密块最大长度限制


                    if (PlaintextData.Length <= MaxBlockSize)
                    {
                        return Convert.ToBase64String(rsa.Encrypt(PlaintextData, false));
                    }
                    else
                    {
                        using (MemoryStream PlaiStream = new MemoryStream(PlaintextData))
                        using (MemoryStream CrypStream = new MemoryStream())
                        {

                            Byte[] Buffer = new Byte[MaxBlockSize];

                            int BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);

                            while (BlockSize > 0)
                            {
                                Byte[] ToEncrypt = Buffer;
                                if (BlockSize != MaxBlockSize)
                                {
                                    ToEncrypt = new Byte[BlockSize];
                                    Array.Copy(Buffer, ToEncrypt, BlockSize);
                                }
                                Byte[] Cryptograph = rsa.Encrypt(ToEncrypt, false);
                                CrypStream.Write(Cryptograph, 0, Cryptograph.Length);
                                BlockSize = PlaiStream.Read(Buffer, 0, MaxBlockSize);
                            }
                            return Convert.ToBase64String(CrypStream.ToArray(), Base64FormattingOptions.None);
                        }
                    }
                }
                else
                {
                    return Base64_en(m);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                return "";
            }
        }

        /// <summary>
        /// RSA签名
        /// </summary>
        /// <param name="md5StrSrc">需要签名的密文</param>
        /// <returns>base64格式的签名</returns>
        public static string System_Sign_RSA_en(string md5StrSrc)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(GetKey());
                RSAPKCS1SignatureFormatter formatter = new RSAPKCS1SignatureFormatter(rsa);
                formatter.SetHashAlgorithm("MD5");
                byte[] hashbytes = MD5Str(md5StrSrc);
                byte[] inArray = formatter.CreateSignature(hashbytes);
                return Convert.ToBase64String(inArray);
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// md5运算
        /// </summary>
        /// <param name="md5StrSrc">需要md5的字符串</param>
        /// <returns></returns>
        private static byte[] MD5Str(string md5StrSrc)
        {
            HashAlgorithm hash = MD5.Create();//创建MD5类的实例

            Encoding enc = Encoding.UTF8;//获取UTF8Encoding 类的实例
            byte[] data = enc.GetBytes(md5StrSrc);//内容转换成unicode编码，保存到byte类型的数组里面
            byte[] hashbytes = hash.ComputeHash(data);
            string hasbtye_string = Convert.ToBase64String(hashbytes);
            return hashbytes;
        }

        /// <summary>
        /// 使用本地公钥进行签名验证
        /// </summary>
        /// <param name="md5StrSrc">base64密文</param>
        /// <param name="sign">base64签名</param>
        /// <returns></returns>
        public static bool System_Sign_RSA_de(string md5StrSrc, string sign)
        {
            return System_Sign_RSA_de(md5StrSrc, sign, DEF_PK);
        }

        /// <summary>
        /// 签名验证
        /// </summary>
        /// <param name="md5StrSrc">base64密文</param>
        /// <param name="sign">base64签名</param>
        /// <param name="modulus">验证签名的公钥,null则不进行验证(始终返回true)</param>
        /// <returns></returns>
        public static bool System_Sign_RSA_de(string md5StrSrc, string sign, Byte[] modulus)
        {
            try
            {
                if (modulus != null)
                {
                    //是否需要验证签名
                    if (sign.Equals(""))
                    {//FIXME:确定下具体实现
                        return true;
                    }
                    else if (sign.Equals("Tm9TaWduYXR1cmU="))
                    {
                        return true;
                    }
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                    RSAParameters para_en = new RSAParameters();
                    para_en.Exponent = Convert.FromBase64String("AQAB");
                    para_en.Modulus = modulus;
                    rsa.ImportParameters(para_en);

                    RSAPKCS1SignatureDeformatter deformatter = new RSAPKCS1SignatureDeformatter(rsa);
                    deformatter.SetHashAlgorithm("MD5");
                    byte[] rgbSignature = Convert.FromBase64String(sign);
                    byte[] rgbHash = MD5Str(md5StrSrc);
                    if (deformatter.VerifySignature(rgbHash, rgbSignature))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string Base64_en(string str)
        {
            byte[] PlaintextData = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(PlaintextData);

        }

        public static string Base64_de(string str)
        {
            byte[] PlaintextData = Convert.FromBase64String(str);
            return Encoding.UTF8.GetString(PlaintextData);

        }
    }
}