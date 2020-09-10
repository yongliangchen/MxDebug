/***
 * 
 *    Title: MXFramework
 *           主题: 资源模块全局定义
 *    Description: 
 *           功能：1.资源模块全局枚举定义
 *                2.资源模块全局委托定义
 *                3.资源模块全局数据定义            
 *                                  
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder:      
 *
 */

using System;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>资源模块全局定义</summary>
    public sealed class AssetDefine
    {
        /// <summary>Unity场景打包AB时候的后缀名称</summary>
        public const string AB_SCENE_EXTENSIONS = "u3d";
        /// <summary>Unity资源打包AB时候的后缀名称</summary>
        public const string AB_RES_EXTENSIONS = "data";

        public const string AB_RESOURCES = "Res/AbRes";

        /// <summary>
        /// 得到Ab资源输入路径
        /// </summary>
        /// <returns>The ABR esource path.</returns>
        public static string GetABResourcePath()
        {
            return Application.dataPath + "/" + AB_RESOURCES;
        }

        /// <summary>
        /// 获取AB资源路径
        /// </summary>
        /// <returns>The ABO ut path.</returns>
        public static string GetABOutPath()
        {
            return GetPlatformPath() + "/AssetsBundles" + "/" + GetPlatformName();
        }

        /// <summary>获取AB资源打包路径</summary>
        public static string GetBuildAssetOutPath()
        {
            return Application.streamingAssetsPath + "/AssetsBundles";
        }

        /// <summary>
        /// 获取平台路径
        /// </summary>
        /// <returns>The platform path.</returns>
        private static string GetPlatformPath()
        {
            string strReturnPlatformPath = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:

                    strReturnPlatformPath = "file://" + Application.streamingAssetsPath;
                    break;

                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:

                    strReturnPlatformPath = Application.streamingAssetsPath;
                    break;
            }

            return strReturnPlatformPath;
        }

        /// <summary>
        /// 获取平台名称
        /// </summary>
        /// <returns>The platform name.</returns>
        public static string GetPlatformName()
        {
            string strReturnPlatformName = string.Empty;

            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:

                    strReturnPlatformName = "Windows";
                    break;

                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:

                    strReturnPlatformName = "OSX";
                    break;

                case RuntimePlatform.IPhonePlayer:

                    strReturnPlatformName = "iOS";
                    break;

                case RuntimePlatform.Android:

                    strReturnPlatformName = "Android";
                    break;
            }

            return strReturnPlatformName;
        }
    }

    /// <summary>Ab参数</summary>
    public struct AbParam
    {
        public string SceneName;
        public string AbName;
        public string AssetName;
    }

    public class AbInfo
    {
        public AbParam AbParam { get; set; }
        public EnumAbState AbState { get; set; }
    }

    /// <summary>Ab资源状态</summary>
    public enum EnumAbState
    {
        /// <summary>未知状态</summary>
        None,
        /// <summary>正在加载当中</summary>
        Loading,
        /// <summary>加载完成</summary>
        LoadFinish,
        /// <summary>正在释放</summary>
        Release,
    }

    /// <summary>AssetBundle资源下载完成回调</summary>
    public delegate void DelLoadComplete(string abName);
}



