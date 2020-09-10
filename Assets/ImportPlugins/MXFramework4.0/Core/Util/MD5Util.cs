/***
 * 
 *    Title: MXFramework
 *           主题: MD5工具类
 *    Description: 
 *           功能：1.获取文件的Md5
 *                2.获取字符串的Md5
 * 
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder: 
 *      
 */

using System;
using System.Security.Cryptography;
using System.IO;

namespace Mx.Util
{
    /// <summary>MD5工具类</summary>
    public class MD5Util
    {
        /// <summary>获取文件的MD5</summary>
        public static string GetFileMd5(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                int len = (int)fs.Length;
                byte[] data = new byte[len];
                fs.Read(data, 0, len);
                fs.Close();
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] result = md5.ComputeHash(data);
                string fileMD5 = "";
                foreach (byte b in result)
                {
                    fileMD5 += Convert.ToString(b, 16);
                }
                return fileMD5;
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

        /// <summary>获取字符串的Md5</summary>
        public static string GetStringMd5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = string.Empty;
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            //使用 PadLeft 和 PadRight 进行轻松地补位
            destString = destString.PadLeft(32, '0');
            return destString;
        }


    }//class_end
}