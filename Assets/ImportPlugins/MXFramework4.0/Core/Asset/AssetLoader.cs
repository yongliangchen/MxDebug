using System.Collections;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>
    /// 加载单个 AssetBundle 资源
    /// </summary>
    public class AssetLoader : System.IDisposable
    {
        private AssetBundle _CurrentAssetBundle;
        private Hashtable _Ht;
      
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abObj">Ab object.</param>
        public AssetLoader(AssetBundle abObj)
        {
            if(abObj!=null)
            {
                _CurrentAssetBundle = abObj;
                _Ht = new Hashtable();
            }

            else 
            {
                Debug.LogWarning(GetType() + "/构造函数 AssetBundle() abObj为空！");
            }
        }

        /// <summary>
        /// 加载当前包中指定资源
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="assetName">加载资源名称</param>
        /// <param name="isCache">是否需要缓存</param>
        public UnityEngine.Object LoadAsset(string assetName,bool isCache=false)
        {
            if (!_CurrentAssetBundle.Contains(assetName))
            {
                Debug.LogError(GetType() + "/LoadAsset()/ load asset error! assetName:" + assetName);
                return null;
            }
            return LoadResource<UnityEngine.Object>(assetName,isCache);
        }


        /// <summary>
        /// 加载当前包中指定资源
        /// </summary>
        /// <returns>The resource.</returns>
        /// <param name="assetName">加载资源名称</param>
        /// <param name="isCache">是否需要缓存</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        private T LoadResource <T>(string assetName,bool isCache) where T:UnityEngine.Object
        {
            if(_Ht.Contains(assetName))
            {
                return _Ht[assetName] as T;
            }

            T tmpTResource = _CurrentAssetBundle.LoadAsset<T>(assetName);

            if(tmpTResource!=null && isCache)
            {
                _Ht.Add(assetName, tmpTResource);
            }
            else if(tmpTResource==null)
            {
                Debug.LogError(GetType() + "/LoadResource<T>() tmpTResource 为空！ assetName="+assetName);
            }

            return tmpTResource;
        }


        /// <summary>
        /// 卸载指定资源
        /// </summary>
        /// <returns>要卸载资源的资源</returns>
        /// <param name="">.</param>
        public bool UnLoadAsset(UnityEngine.Object asset)
        {
            if(asset!=null)
            {
                Resources.UnloadAsset(asset);
                return true;
            }

            Debug.LogError(GetType() + "/UnLoadAsset()/ 参数asse为空！");

            return false;
        }


        /// <summary>
        /// 释放当前 AssetBundle 内存镜像资源
        /// </summary>
        public void Dispose()
        {

            _CurrentAssetBundle.Unload(false);
        }

        /// <summary>
        /// 释放当前 AssetBundle 内存镜像资源切释放内存资源
        /// </summary>
        public void DisposeALL()
        {
            _CurrentAssetBundle.Unload(true);
        }


        /// <summary>
        /// 查询当前 AssetBundle 包含的所有资源
        /// </summary>
        /// <returns>The all asset name.</returns>
        public string[] RetriveAllAssetName()
        {
            return _CurrentAssetBundle.GetAllAssetNames();
        }
    }

}

