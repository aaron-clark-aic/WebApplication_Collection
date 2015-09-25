using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace WebApplication4.Models.ECC
{
    public class EccSign
    {
        /// <summary>
        /// 使用公钥验证签名
        /// </summary>
        public static bool VerifyData(byte[] data, byte[] signature, byte[] publicKey)
        {
            //验证是否签名匹配
            bool verified = false;
            // 导入公钥
            using (CngKey cngKey = CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob))
            {
                // 验证签名
                using (ECDsaCng ecdsa = new ECDsaCng(cngKey))
                {
                    verified = ecdsa.VerifyData(data, signature);
                    return verified;
                }
            }
        }

        /// <summary>
        /// 使用私钥签名
        /// </summary>
        public static byte[] SignData(byte[] data, byte[] privateKey)
        {
            ///是否能够完全的释放using,在return以后
            // 打开密钥
            using (CngKey cngKey = CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob))
            {
                // 签名
                using (ECDsaCng ecdsa = new ECDsaCng(cngKey))
                {
                    byte[] signature = ecdsa.SignData(data);
                    return signature;
                }
            }
        }
    }
}