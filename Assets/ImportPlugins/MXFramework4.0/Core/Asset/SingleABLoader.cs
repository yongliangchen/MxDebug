using System.Collections;
using UnityEngine;

namespace Mx.Res
{

    /// <summary>
    /// 加载单个AssetBunlde
    /// </summary>
    public class SingleABLoader : System.IDisposable
    {
        private AssetLoader _AssetLoader;

        private DelLoadComplete _LoadCompleteHandle;

        /// <summary>资源名称</summary>
        private string _ABName;
        /// <summary>资源下载路径</summary>
        private string _ABDownLoadPath;

        //构造函数
        public SingleABLoader(string abName,DelLoadComplete loadComplete)
        {
            _AssetLoader = null;
            _ABName = abName;

            _LoadCompleteHandle = loadComplete;

            _ABDownLoadPath = AssetDefine.GetABOutPath()+"/"+ abName;
        }

        /// <summary>
        /// 加载AssetBundle 资源包
        /// </summary>
        /// <returns>The asset bunlde.</returns>
        public IEnumerator LoadAssetBunlde()
        {
            using (WWW www = new WWW(_ABDownLoadPath))
            {
                yield return www;

                if (www.progress >= 1)
                {
                    AssetBundle abObj = www.assetBundle;

                    if (abObj != null)
                    {
                        _AssetLoader = new AssetLoader(abObj);

                        if (_LoadCompleteHandle != null) _LoadCompleteHandle(_ABName);
                    }
                    else
                    {
                        Debug.LogError(GetType() + "/LoadAssetBunlde()/ www下载失败！ AssetBundleUrl:" + _ABDownLoadPath + "  错误信息：" + www.error);
                    }
                }
            }
        }

        /// <summary>
        /// 加载（Ab包内资源）
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="assetName">资源名称</param>
        public UnityEngine.Object LoadAsset(string assetName)
        {
            if(_AssetLoader!=null)
            {
                return _AssetLoader.LoadAsset(assetName);
            }

            Debug.LogError(GetType() + "LoadAsset()/ 参数_AssetLoader为空！ assetName:"+ assetName);

            return null;
        }


        /// <summary>
        /// 卸载（AB包中资源）
        /// </summary>
        /// <param name="asset">Asset.</param>
        public void UnLoadAsset(UnityEngine.Object asset)
        {
            if(_AssetLoader!=null)
            {
                _AssetLoader.UnLoadAsset(asset);
            }
            else 
            {
                Debug.LogError(GetType() + "/UnLoadAsset()/参数_AssetLoader为空！");
            }
        }


        /// <summary>
        /// 卸载资源
        /// </summary>
        public void Dispose()
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.Dispose();
                _AssetLoader = null;
            }
            else
            {
                Debug.LogError(GetType() + "/Dispose()/参数_AssetLoader为空！");
            }
        }

        /// <summary>
        /// 释放当前AssetBundle 资源包，且卸载所有资源
        /// </summary>
        public void DisposeALL()
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.DisposeALL();
                _AssetLoader = null;
            }
            else
            {
                Debug.LogError(GetType() + "/DisposeALL()/参数_AssetLoader为空！");
            }
        }

        /// <summary>
        /// 查询当前AssetBundle中所有资源名称
        /// </summary>
        /// <returns>The de.</returns>
        public string[] RetriveAllAssetName()
        {
            if (_AssetLoader != null)
            {
                return _AssetLoader.RetriveAllAssetName();

            }

            Debug.LogError(GetType() + "/RetriveAllAssetName()/参数_AssetLoader为空！");

            return null;
        }

    }
}

