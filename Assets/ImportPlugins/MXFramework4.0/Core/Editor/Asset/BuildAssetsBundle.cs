using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Mx.Res
{

    public class BuildAssetsBundle
    {

        [MenuItem("MXFramework/AssetBundle/Build AssetBundles/Android", false, 1)]
        public static void BuildAndroidAssetBundles()
        {
            BuildPlatformDirectory(BuildTarget.Android);
        }

        [MenuItem("MXFramework/AssetBundle/Build AssetBundles/IOS", false, 2)]
        public static void BuildiOSAssetBundles()
        {
            BuildPlatformDirectory(BuildTarget.iOS);
        }

        [MenuItem("MXFramework/AssetBundle/Build AssetBundles/Windows", false, 3)]
        public static void BuildWindowsAssetBundles()
        {
            BuildPlatformDirectory(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("MXFramework/AssetBundle/Build AssetBundles/Mac",false, 4)]
        public static void BuildMacAssetBundles()
        {
            BuildPlatformDirectory(BuildTarget.StandaloneOSX);
        }

        /// <summary>
        /// 根据选择平台进行打包
        /// </summary>
        /// <param name="buildTarget">打包平台的名称</param>
        private static void BuildPlatformDirectory(BuildTarget buildTarget)
        {
            string targetName = string.Empty;
            switch (buildTarget)
            {

                case BuildTarget.Android:

                    targetName = "Android";
                    break;

                case BuildTarget.iOS:

                    targetName = "iOS";
                    break;

                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneWindows:

                    targetName = "Windows";
                    break;

                case BuildTarget.StandaloneOSX:

                    targetName = "OSX";
                    break;

                default:

                    Debug.LogError("打包的平台不存在! buildTarget=" + buildTarget);
                    break;
            }

            string outPath = AssetDefine.GetBuildAssetOutPath() + "/" + targetName;

            AutoSetLabels.SetAbLabel();//设置标签

            if (!Directory.Exists(outPath))
                Directory.CreateDirectory(outPath);

            BuildPipeline.BuildAssetBundles(outPath, 0, buildTarget);
            AssetDatabase.Refresh();//刷新
            Debug.Log("打包资源成功! outPath: " + outPath);

            GenerateAssetList.CreateFiles(outPath);//生成资源清单

        }

    }
}

