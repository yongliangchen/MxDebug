/***
 * 
 *    Title: MXFramework
 *           主题: Directory类扩展工具
 *    Description: 
 *           功能：1.Directory常用的API进行封装
 * 
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder: 
 *      
 */

using System.Collections.Generic;

namespace System.IO
{

    public delegate bool DelCondition<FileInfo>(FileInfo fileInfo);

    /// <summary>Directory类扩展</summary>
    public static class DirectoryEx
    {

        /// <summary>
        /// 读取目录中的文件
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="condition">添加筛选条件</param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string folderPath, DelCondition<FileInfo> condition)
        {
            FileInfo[] temp = GetFiles(folderPath);
            List<FileInfo> fileInfos = new List<FileInfo>();

            for(int i=0;i< temp.Length;i++)
            {
                if (!condition(temp[i])) continue;
                fileInfos.Add(temp[i]);
            }

            return fileInfos.ToArray();
        }

        /// <summary>
        /// 读取目录中的文件
        /// </summary>
        /// <param name="folderPath">文件夹路径</param>
        /// <returns></returns>
        public static FileInfo[] GetFiles(string folderPath, string searchPattern = "*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            //获取指定路径下面的所有资源文件  
            if (Directory.Exists(folderPath))
            {
                DirectoryInfo direction = new DirectoryInfo(folderPath);
                FileInfo[] files = direction.GetFiles(searchPattern, searchOption);
                return files;
            }

            else return null;
        }

    }
}