using System;
using Mx.Util;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>AssetBundel资源管理</summary>
    public class AssetManager : MonoSingleton<AssetManager>
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);//加载场景的时候不销毁
        }

        public void LoadAssetBundlePack(AbParam abParam, Action finish)
        {
            AssetBundleMgr.Instance.LoadAssetBundlePack(abParam.SceneName, abParam.AbName, (p) =>{finish();});
        }

        public void LoadAsset(AbParam abParam, Action<string, UnityEngine.Object> finish)
        {
            AssetBundleMgr.Instance.LoadAssetBundlePack(abParam.SceneName, abParam.AbName, (p) =>
            {
                UnityEngine.Object asset = AssetBundleMgr.Instance.LoadAsset(abParam.SceneName, abParam.AbName, abParam.AssetName);
                if (asset == null) finish("load assetBundel errro!", null);
                else finish(null, asset);
            });
        }

        public void Dispose(string sceneName)
        {
            AssetBundleMgr.Instance.Dispose(sceneName);
        }

        public void DisposeAllAssetBundle()
        {
            AssetBundleMgr.Instance.DisposeAllAssetBundle();
        }
    }
}