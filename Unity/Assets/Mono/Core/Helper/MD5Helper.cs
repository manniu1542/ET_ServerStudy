using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ET
{
    public static class MD5Helper
    {
        public static string FileMD5(string filePath)
        {
            byte[] retVal;
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                MD5 md5 = MD5.Create();
                retVal = md5.ComputeHash(file);
            }
            return retVal.ToHex("x2");
        }
        public static string StringMD5(string content)
        {
            string strMd5 = string.Empty;
            // 创建MD5实例
            using (MD5 md5 = MD5.Create())
            {
                // 将输入字符串转换为字节数组
                byte[] inputBytes = Encoding.UTF8.GetBytes(content);

                // 计算哈希值
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // 将字节数组转换为十六进制字符串
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                strMd5 = sb.ToString(); // 返回哈希后的字符串
            }
            return strMd5;
        }
    }
}
