/***
 * 
 *    Title: MXFramework
 *           主题: 生成资源清单
 *    Description: 
 *           功能：自动生成资源清单
 * 
 *    Date: 2019
 *    Version: v1.3.0版本
 *    Modify Recoder: 
 *      
 */

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>
    /// 生成资源清单
    /// </summary>
    public class GenerateAssetList
    {
        /// <summary>
        /// 生成资源清单
        /// </summary>
        /// <param name="outPath">Out path.</param>
        public static void CreateFiles(string outPath)
        {
            //校验文件的路径
            string filePath = outPath + "/files.txt";
            if (File.Exists(filePath))
                File.Delete(filePath);

            //遍历这个文件夹下面的所有文件 
            List<string> fileList = new List<string>();
            listFiles(new DirectoryInfo(outPath), ref fileList);

            FileStream fs = new FileStream(filePath, FileMode.CreateNew);
            StreamWriter sw = new StreamWriter(fs);

            for (int i = 0; i < fileList.Count; i++)
            {
                string file = fileList[i];
                string ext = Path.GetExtension(file);
                if (ext.EndsWith(".meta"))
                    continue;

                FileInfo fileInfo = new FileInfo(file);

                //生成这个文件对应的md5值 
                string md5 = Mx.Util.MD5Util.GetFileMd5(file);

                string value = file.Replace(outPath + "/", string.Empty);
                //写入到文件
                sw.WriteLine(value + "|" + md5 + "|" + fileInfo.Length);
            }

            sw.Close();
            fs.Close();

            Debug.Log("生成资源清单完成! OutPath:" + outPath);
            AssetDatabase.Refresh();//刷新
        }


        /// <summary>
        /// 遍历文件夹下的所有文件
        /// </summary>
        /// <param name="fileSystemInfo">文件夹的路径</param>
        /// <param name="fileList"></param>
        private static void listFiles(FileSystemInfo fileSystemInfo, ref List<string> fileList)
        {
            DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
            //获取所有的文件系统
            FileSystemInfo[] infos = directoryInfo.GetFileSystemInfos();

            foreach (var info in infos)
            {
                FileInfo fileInfo = info as FileInfo;
                //如果是文件 就成功了
                if (fileInfo != null)
                {
                    fileList.Add(fileInfo.FullName.Replace("\\", "/"));
                }
                //如果是文件夹就不成功 null
                else
                {
                    //递归
                    listFiles(info, ref fileList);
                }
            }
        }

    }

}

