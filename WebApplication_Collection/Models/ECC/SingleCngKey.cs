using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace WebApplication4.Models.ECC
{
    /// <summary>
    /// 用于提供服务器密钥管理的单例
    /// </summary>
    public class SingleCngKey
    {
        private static SingleCngKey _instance = null;

        private CngKey ck;

        /// <summary>
        /// [aaron.clark.aic][2015-06-10][需要验证为什么CngKey会把当前实例保存下来?存放方法和位置]
        /// </summary>
        private SingleCngKey() {
            CngKeyCreationParameters creationParameters = new CngKeyCreationParameters();
            // 允许以明文的形式导出私钥
            creationParameters.ExportPolicy = CngExportPolicies.AllowPlaintextExport;
            if (!CngKey.Exists("ecc"))
            {
                ck = CngKey.Create(CngAlgorithm.ECDsaP521, "ecc", creationParameters);
            }
            else {
                ck = CngKey.Open("ecc");
            }
        }

        public static SingleCngKey GetInstance() {
            if (_instance != null)
            {
                return _instance;
            }
            else {
                return new SingleCngKey();
            }
        }

        public byte[] GetEccPublicKey() {
            return ck.Export(CngKeyBlobFormat.EccPublicBlob);
        }

        public byte[] GetEccPrivateKey() {
            return ck.Export(CngKeyBlobFormat.EccPrivateBlob);
        }
    }
}