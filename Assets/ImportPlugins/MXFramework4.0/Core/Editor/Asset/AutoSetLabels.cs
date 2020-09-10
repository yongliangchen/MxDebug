using UnityEngine;
using UnityEditor;
using System.IO;

namespace Mx.Res
{
    public class AutoSetLabels
    {
        /// <summary>
        /// 自动设置标记
        /// </summary>
        //[MenuItem("MXFramework/AssetBundle/Set AssetBundle Label",false,0)]
        public static void SetAbLabel()
        {
            //需要打包AssetBunld的跟目录
            //string strNeedSetLabelRoot = Application.dataPath + "/Res/AbRes";

            string strNeedSetLabelRoot = AssetDefine.GetABResourcePath();

            //如果打包路径不存在者创建
            if(!Directory.Exists(strNeedSetLabelRoot))
            {
                Directory.CreateDirectory(strNeedSetLabelRoot);
            }

            //目录信息（场景中的目录信息数组）
            DirectoryInfo[] dirScenesDIRArr = null;

            DirectoryInfo dirTempInfo = new DirectoryInfo(strNeedSetLabelRoot);
            dirScenesDIRArr = dirTempInfo.GetDirectories();

            foreach(DirectoryInfo currentDIR in dirScenesDIRArr)
            {
                string tempScenesDIR = strNeedSetLabelRoot + "/" + currentDIR.Name;
                //DirectoryInfo tempScenesDIRInfo = new DirectoryInfo(tempScenesDIR);

                int tempIndex = tempScenesDIR.LastIndexOf("/");
                string tmpScenesName = tempScenesDIR.Substring(tempIndex + 1);

                JudgeDIRorFileBuyRecursive(currentDIR, tmpScenesName);
            }

            //清空无用AB标记
            AssetDatabase.RemoveUnusedAssetBundleNames();

            AssetDatabase.Refresh();//刷新

           Debug.Log("自动设置标志完成！");
        }

        /// <summary>
        /// 递归判断是目录还是文件，修改AssetBundle标记
        /// </summary>
        /// <param name="fileSystemInfo"> fileSystemInfo </param>
        /// <param name="scenesName">Scenes name.</param>
        private static void JudgeDIRorFileBuyRecursive(FileSystemInfo fileSystemInfo,string scenesName)
        {
            if(!fileSystemInfo.Exists)
            {
               Debug.LogError("文件或者目录名称： " + fileSystemInfo + " 不存在！");
                return;
            }

            DirectoryInfo dirInfoObj = fileSystemInfo as DirectoryInfo;
            FileSystemInfo[] fileSysArray = dirInfoObj.GetFileSystemInfos();

            foreach(FileSystemInfo fileInfo in fileSysArray)
            {

                FileInfo fileInfoObj = fileInfo as FileInfo;

                //文件类型
                if(fileInfoObj!=null)
                {
                    SetFileABLabel(fileInfoObj, scenesName);
                }

                //目录类型
                else
                {
                    JudgeDIRorFileBuyRecursive(fileInfo, scenesName);
                }
            }
        }

        /// <summary>
        /// 对指定文件设置AB包名
        /// </summary>
        /// <param name="fileInfoObj">File info object.</param>
        /// <param name="scenesName">Scenes name.</param>
        private static void SetFileABLabel(FileInfo fileInfoObj, string scenesName)
        {
            if (GetIgnoreFile(fileInfoObj)) return;

            string strABName = string.Empty;
            string strAssetFilePath = string.Empty;

            strABName = GetABName(fileInfoObj, scenesName);

            int tmpIndex = fileInfoObj.FullName.IndexOf("Assets");
            strAssetFilePath = fileInfoObj.FullName.Substring(tmpIndex);
            AssetImporter tmpInmporterObj = AssetImporter.GetAtPath(strAssetFilePath);
            tmpInmporterObj.assetBundleName = strABName;

            if(fileInfoObj.Extension==".unity")
            {
                tmpInmporterObj.assetBundleVariant = "u3d";
            }
            else
            {
                tmpInmporterObj.assetBundleVariant = "data";
            }
        }

        /// <summary>
        /// 获取Ab包名
        /// </summary>
        /// <returns>The ab name.</returns>
        /// <param name="fileInfo">File info.</param>
        /// <param name="sceneName">Scene name.</param>
        private static string GetABName(FileInfo fileInfoObj, string sceneName)
        {
            string strABName = string.Empty;

            string tmpWinPath = fileInfoObj.FullName;
            string tmpUnityPath = tmpWinPath.Replace("\\", "/");//替换为Unity路径

            int tmpSceneNamePosition = tmpUnityPath.IndexOf(sceneName) + sceneName.Length;
            string strABFileNameArea = tmpUnityPath.Substring(tmpSceneNamePosition + 1);

            if (strABFileNameArea.Contains("/"))
            {
                string[] tmpStrArray = strABFileNameArea.Split('/');
                strABName = sceneName + "/" + tmpStrArray[0];
            }
            else
            {
                strABName = sceneName + "/" + sceneName;
            }

            return strABName;
        }

        /// <summary>获取需要忽略的文件</summary>
        private static bool GetIgnoreFile(FileInfo fileInfo)
        {
            if (fileInfo.Extension == ".meta" || fileInfo.Extension == ".DS_Store" || fileInfo.Extension == ".cs" ||
                fileInfo.Extension == ".dll" || fileInfo.Extension == ".cpp" || fileInfo.Extension == ".a"
                || fileInfo.Extension == ".so"

               ) return true;

            else return false;
        }

        #region 清空所以标记

        [MenuItem("MXFramework/AssetBundle/Clear All AssetBundles Labels",false,5)]
        public static void ClearAbLabels()
        {
            string path = Application.dataPath;
            string[] files = System.IO.Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            if ((files != null) && (files.Length > 0))
            {
                for (int i = 0; i < files.Length; ++i)
                {
                    string fileName = files[i];

                    FileInfo fileInfo = new FileInfo(fileName);

                    if (!GetIgnoreFile(fileInfo))
                    {
                        int index = fileName.IndexOf("Assets");
                        fileName = fileName.Substring(index);
                        AssetImporter importer = AssetImporter.GetAtPath(fileName);

                        if (importer != null) importer.assetBundleName = string.Empty;
                    }
                }
            }

            AssetDatabase.RemoveUnusedAssetBundleNames();
            AssetDatabase.Refresh();
           Debug.Log("清空 AssetBundle Labels 完成！");
        }

        #endregion
    }
}
