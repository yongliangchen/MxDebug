using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.Util;

namespace Mx.Res
{
    /// <summary>
    /// 管理整个AssetBundle (AB程序入口)
    /// </summary>
    public class AssetBundleMgr : MonoSingleton<AssetBundleMgr>
    {
        /// <summary>场景集合</summary>
        private Dictionary<string, MultiABMgr> dicAllScenes = new Dictionary<string, MultiABMgr>();
        /// <summary>加载过了的资源清单</summary>
        private Dictionary<string, AbInfo> dicAbCach = new Dictionary<string, AbInfo>();
        /// <summary>清单文件</summary>
        private AssetBundleManifest manifestObj = null;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            //加载Manifest文件
            StartCoroutine(ABManifestLoader.Instance.LoadManifestFile());
        }

        /// <summary>
        /// 下载Ab指定包
        /// </summary>
        /// <returns>The asset bundle pack.</returns>
        /// <param name="sceneName">场景名字</param>
        /// <param name="abName">Ab包名</param>
        /// <param name="loadAllCompleteHandle">下载完成回调.</param>
        public void LoadAssetBundlePack(string sceneName, string abName, DelLoadComplete loadAllCompleteHandle)
        {
            StartCoroutine(LoadAssetBundlePackAsyn(sceneName, abName, loadAllCompleteHandle));
        }

        private IEnumerator LoadAssetBundlePackAsyn(string sceneName, string abName, DelLoadComplete loadAllCompleteHandle)
        {
            AbInfo abInfo = null;

            //等待Manifest文件加载完成
            while (!ABManifestLoader.Instance.IsLoadFinish) yield return null;
            manifestObj = ABManifestLoader.Instance.GetABManifest();
            if (manifestObj == null)
            {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/manifestObj is Null!");
                yield break;
            }

            abName = abName.ToLower();//将名字转换成小写

            if (dicAbCach.ContainsKey(abName))
            {
                dicAbCach.TryGetValue(abName, out abInfo);

                if (abInfo.AbState == EnumAbState.Release)
                {
                    Debug.Log("资源正在释放！abName:" + abName);
                    yield break;
                }

                while (abInfo.AbState == EnumAbState.Loading) yield return null;

                loadAllCompleteHandle(abName);
                yield break;
            }
            else
            {
                abInfo = new AbInfo();
                AbParam abParam = new AbParam();
                abParam.SceneName = sceneName;
                abParam.AbName = abName;

                abInfo.AbParam = abParam;
                abInfo.AbState = EnumAbState.Loading;

                dicAbCach.Add(abName, abInfo);
            }

            if (!dicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = new MultiABMgr(sceneName, abName, loadAllCompleteHandle);
                dicAllScenes.Add(sceneName, multiABMgrObj);
            }

            MultiABMgr tmpMultiABMgr = dicAllScenes[sceneName];
            if (tmpMultiABMgr == null)
            {
                Debug.LogError(GetType() + "/LoadAssetBundlePack()/tmpMultiABMgr is Null!");
            }

            yield return tmpMultiABMgr.LoadAssetBundle(abName);

            abInfo.AbState = EnumAbState.LoadFinish;
        }

        /// <summary>
        /// 加载Ab包中资源
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="assetName">资源名称</param>
        public UnityEngine.Object LoadAsset(string sceneName, string abName, string assetName)
        {
            //将名字转换成小写
            abName = abName.ToLower();

            if (dicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = dicAllScenes[sceneName];
                return multiABMgrObj.LoadAsset(abName, assetName);
            }

            Debug.LogWarning(GetType() + "/LoadAsset()/找不到场景名称，无法加载（AB包）中资源！ sceneName=" + sceneName);

            return null;
        }

        /// <summary>
        /// 释放一个场景里面所有资源
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        public void Dispose(string sceneName)
        {
            List<string> disposeAb = new List<string>();

            foreach (string key in dicAbCach.Keys)
            {
                string tempSceneName = dicAbCach[key].AbParam.SceneName;
                if (tempSceneName.Equals(sceneName))
                {
                    dicAbCach[key].AbState = EnumAbState.Release;
                    disposeAb.Add(key);
                }
            }

            if (dicAllScenes.ContainsKey(sceneName))
            {
                MultiABMgr multiABMgrObj = dicAllScenes[sceneName];
                multiABMgrObj.DisposeAllAsset();
                dicAllScenes.Remove(sceneName);
            }

            else { Debug.LogWarning(GetType() + "/DisposeAllAssets()/找不到场景名,释放资源失败！ sceneName=" + sceneName); }

            for (int i = 0; i < disposeAb.Count; i++) { dicAbCach.Remove(disposeAb[i]); }
        }

        /// <summary>释放全部AssetBundle资源</summary>
        public void DisposeAllAssetBundle()
        {
            Debug.Log("AssetBundle资源回收!");
            dicAllScenes.Clear();
            AssetBundle.UnloadAllAssetBundles(false);
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

    }
}
