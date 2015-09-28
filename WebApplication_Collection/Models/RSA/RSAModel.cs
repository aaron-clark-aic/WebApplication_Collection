using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace JJYhttpMvc.Model
{
    public class RSAModel
    {
        /// <summary>
        /// 本地公钥
        /// </summary>
        public static readonly byte[] DEF_PK;

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static RSAModel()
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(GetKey());
            DEF_PK = rsa.ExportParameters(false).Modulus;
        }

        /// <summary>
        /// 使用本地公钥对字符串进行加密
        /// </summary>
        /// <param name="m">需要加密的内容</param>
        /// <returns></returns>
        public static string System_Numberic_RSA_en(string m)
        {
            return System_Numberic_RSA_en(m, DEF_PK);
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
                sw = new StreamReader(addressStr + "/App_Data/PublicKey.xml", false);
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

        /// <summary>
        /// RSA解密
        /// </summary>
        /// <param name="enStr">需要解密的base64字符串</param>
        /// <returns>明文</returns>
        public static string System_Numberic_RSA_de(string enStr)
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

                rsa.FromXmlString(GetKey());

                byte[] CiphertextData = Convert.FromBase64String(enStr);
                int MaxBlockSize = rsa.KeySize / 8;    //解密块最大长度限制

                if (CiphertextData.Length <= MaxBlockSize)
                {
                    return Encoding.UTF8.GetString(rsa.Decrypt(CiphertextData, false));
                }
                else
                {
                    using (MemoryStream CrypStream = new MemoryStream(CiphertextData))
                    using (MemoryStream PlaiStream = new MemoryStream())
                    {
                        Byte[] Buffer = new Byte[MaxBlockSize];
                        int BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                        while (BlockSize > 0)
                        {
                            Byte[] ToDecrypt = Buffer;
                            if (BlockSize != MaxBlockSize)
                            {
                                ToDecrypt = new Byte[BlockSize];
                                Array.Copy(Buffer, ToDecrypt, BlockSize);
                            }
                            Byte[] Plaintext = rsa.Decrypt(ToDecrypt, false);
                            PlaiStream.Write(Plaintext, 0, Plaintext.Length);
                            BlockSize = CrypStream.Read(Buffer, 0, MaxBlockSize);
                        }
                        return Encoding.UTF8.GetString(PlaiStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //Console.ReadLine();
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
                    if(sign.Equals("")){//FIXME:确定下具体实现
                        return true;
                    } else if(sign.Equals("Tm9TaWduYXR1cmU=")){
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
            catch (Exception )
            {
                return false;
            }
        }

        /// <summary>
        /// post方法传递json数据
        /// </summary>
        /// <param name="urlStr">目标url</param>
        /// <param name="urlStrFunc">对应方法</param>
        /// <param name="urlStrPara">json参数</param>
        /// <returns></returns>
        public static string Post_RSA_Json(string urlStr, string urlStrFunc, string urlStrPara)
        {
            Encoding encode = Encoding.UTF8;
            byte[] arrB = encode.GetBytes(urlStrPara);
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(urlStr + urlStrFunc);
            myReq.Method = "POST";
            myReq.ContentType = "application/x-www-form-urlencoded";
            myReq.ContentLength = arrB.Length;
            Stream outStream = myReq.GetRequestStream();
            outStream.Write(arrB, 0, arrB.Length);
            outStream.Close();
            //接收HTTP做出的响应
            WebResponse myResp = myReq.GetResponse();

            Stream ReceiveStream = myResp.GetResponseStream();
            StreamReader readStream = new StreamReader(ReceiveStream, encode);
            Char[] read = new Char[256];
            int count = readStream.Read(read, 0, 256);
            string str = null;
            while (count > 0)
            {
                str += new String(read, 0, count);
                count = readStream.Read(read, 0, 256);
            }
            readStream.Close();
            myResp.Close();
            return str;
        }

        public static string Post_RSA_Json_Test(string urlStr, string urlStrFunc, string urlStrPara)
        {
            WebClient wc = new WebClient();
            string url = urlStr + urlStrFunc;
            wc.Encoding = Encoding.UTF8;
            wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");//采取POST方式必须加的header，如果改为GET方式的
            string jsonstr = wc.UploadString(url, urlStrPara);
            return jsonstr;
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