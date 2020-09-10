 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Mx.Res
{
    /// <summary>
    /// 管理Manifest文件加载
    /// </summary>
    public class ABManifestLoader : System.IDisposable
    {
        private static ABManifestLoader instance;
        private AssetBundleManifest manifestObj;
        private string strManifestPath;
        private AssetBundle aBReadManifest;
        private bool isLoadFinish;
        /// <summary>是否加载完成</summary>
        public bool IsLoadFinish
        {
            get { return isLoadFinish; }
        }

        /// <summary>构造函数</summary>
        private ABManifestLoader()
        {
            strManifestPath = AssetDefine.GetABOutPath() + "/" + AssetDefine.GetPlatformName();
            manifestObj = null;
            aBReadManifest = null;
            isLoadFinish = false;
        }

        /// <summary>
        /// 获取本类实例
        /// </summary>
        /// <returns>The instance.</returns>
        public static ABManifestLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ABManifestLoader();
                }

                return instance;
            }
        }


        /// <summary>
        /// 加载Manifast文件
        /// </summary>
        /// <returns>The manifest file.</returns>
        public IEnumerator LoadManifestFile()
        {

            using (WWW www = new WWW(strManifestPath))
            {
                yield return www;

                if (www.progress >= 1)
                {

                    AssetBundle abObj = www.assetBundle;

                    if (abObj != null)
                    {
                        aBReadManifest = abObj;
                        manifestObj = aBReadManifest.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
                        isLoadFinish = true;
                    }

                    else
                    {
                       Debug.LogError(GetType() + "/LoadManifestFile()/加载失败！  strManifestPath= "+strManifestPath+"   错误信息："+www.error);
                    }
                }
            }
        }

        /// <summary>
        /// 获取Manifest文件
        /// </summary>
        /// <returns>The ABM anifest.</returns>
        public AssetBundleManifest GetABManifest()
        {

            if(isLoadFinish)
            {

                if(manifestObj!=null)
                {
                    return manifestObj;
                }

                else 
                {
                   Debug.Log(GetType() + "/GetABManifest()/ manifestObj==Null");
                }
            }

            else 
            {
               Debug.Log(GetType() + "/GetABManifest()/ isLoadFinish==false , Manifest没有加载完成！");
            }

            return null;
        }

        /// <summary>
        /// 获取AssetBundleManifest(系统类)所有依赖项
        /// </summary>
        /// <returns>The dependce.</returns>
        /// <param name="abName">Ab name.</param>
        public string[] RetrivalDependce(string abName)
        {

            if(manifestObj!=null && !string.IsNullOrEmpty(abName))
            {
                return manifestObj.GetAllDependencies(abName);
            }

            return null;
        }
 

        /// <summary>
        /// 资源卸载
        /// </summary>
        public void Dispose()
        {
            if(aBReadManifest!=null)
            {
                aBReadManifest.Unload(true);
            }
        }


    }
}

