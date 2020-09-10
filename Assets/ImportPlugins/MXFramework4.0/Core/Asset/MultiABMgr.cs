using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mx.Res
{
    /// <summary>
    /// 多个AssetBundle管理
    /// </summary>
    public class MultiABMgr
    {
        /// <summary>单个AssetBundle加载实现</summary>
        private SingleABLoader _CurrentSingleABLoader;
        /// <summary>加载缓存集合</summary>
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;
        /// <summary>当前场(调试时候用)</summary>
        //private string _CurrentSceneName;
        /// <summary>当前的AssetBundle名称</summary>
        private string _CurrentABName;
        /// <summary>对应的引用关系</summary>
        private Dictionary<string, ABRelating> _DicABRelating;
        /// <summary>委托所有AB包加载完成</summary>
        private event DelLoadComplete _onLandCompleteEvent;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="abName">Ab包名称</param>
        /// <param name="onLandCompleteEvent">(委托)是否调用完成</param>
        public MultiABMgr(string sceneName,string abName,DelLoadComplete onLandCompleteEvent)
        {
            //_CurrentSceneName = sceneName;
            _CurrentABName = abName;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelating = new Dictionary<string, ABRelating>();
            _onLandCompleteEvent = onLandCompleteEvent;
        }


        /// <summary>
        /// 完成指定AB包调用
        /// </summary>
        /// <param name="abName">Ab name.</param>
        private void CompleteLoadAb(string abName)
        {
            if(abName.Equals(_CurrentABName))
            {
                if(_onLandCompleteEvent!=null)
                {
                    _onLandCompleteEvent(abName);
                }
            }
        }

        /// <summary>
        /// 加载AssetBundle包
        /// </summary>
        /// <returns>The asset bundle.</returns>
        /// <param name="abName">加载AssetBundle包名称</param>
        public IEnumerator LoadAssetBundle(string abName)
        {
            if(!_DicABRelating.ContainsKey(abName))
            {
                ABRelating aBRelatingObj = new ABRelating(abName);
                _DicABRelating.Add(abName, aBRelatingObj);
            }

            ABRelating tmpABRelatingObj = _DicABRelating[abName];

            string[] strDependeceArrar = ABManifestLoader.Instance.RetrivalDependce(abName);
            foreach(string item_Dependece in strDependeceArrar)
            {
                //添加依赖项
                tmpABRelatingObj.AddDependence(item_Dependece);

                //添加引用项
                yield return LoadReference(item_Dependece, abName);
            }

            if(_DicSingleABLoaderCache.ContainsKey(abName))
            {
                yield return _DicSingleABLoaderCache[abName].LoadAssetBunlde();
            }
            else 
            {
                _CurrentSingleABLoader = new SingleABLoader(abName,CompleteLoadAb);
                _DicSingleABLoaderCache.Add(abName, _CurrentSingleABLoader);
                yield return _CurrentSingleABLoader.LoadAssetBunlde();
            }
        }

        /// <summary>
        /// 加载引用AB包
        /// </summary>
        /// <returns>The reference.</returns>
        /// <param name="abName">AB包名称</param>
        /// <param name="refABName">被引用AB包名称</param>
        private IEnumerator LoadReference(string abName,string refABName)
        {
            if(_DicABRelating.ContainsKey(abName))
            {
                ABRelating tmpABRelatingObj = _DicABRelating[abName];
                tmpABRelatingObj.AddReference(refABName);
            }

            else 
            {
                ABRelating tmpABRelatingObj = new ABRelating(abName);
                tmpABRelatingObj.AddReference(refABName);
                _DicABRelating.Add(abName, tmpABRelatingObj);

                yield return LoadAssetBundle(abName);
            }
        }

        /// <summary>
        /// 加载（AB包）中资源
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="abName">AssetBundle名称</param>
        /// <param name="assetName">加载资源名称</param>
        public UnityEngine.Object LoadAsset(string abName,string assetName)
        {
            foreach(string item_abName in _DicSingleABLoaderCache.Keys)
            {
                if(abName==item_abName)
                {
                    return _DicSingleABLoaderCache[item_abName].LoadAsset(assetName);
                }
            }

            Debug.LogError(GetType() + "/LoadAsset()/找不到AssetBundle包，abName=" + abName + "  assetName=" + assetName);

            return null;
        }

        /// <summary>
        /// 释放本场景中的所有资源
        /// </summary>
        public void DisposeAllAsset()
        {
            try
            {
                foreach(SingleABLoader item_ABLoader in _DicSingleABLoaderCache.Values)
                {
                    item_ABLoader.DisposeALL();
                }
            }

            finally
            {
                _DicSingleABLoaderCache.Clear();
                _DicSingleABLoaderCache = null;

                //释放其他对象占用资源
                _DicABRelating.Clear();
                _DicABRelating = null;
                _CurrentABName = null;
                //_CurrentSceneName = null;
                _onLandCompleteEvent = null;

                //卸载没有使用的资源
                Resources.UnloadUnusedAssets();
                //垃圾回收
                System.GC.Collect();
            }

        }

    }
}
