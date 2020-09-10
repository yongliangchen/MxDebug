/***
 * 
 *    Title: MXFramework
 *           主题: Resouces类的封装
 *    Description: 
 *           功能：添加了缓存功能
 *                                 
 *    Date: 2019
 *    Version: v1.3.0版本
 *    Modify Recoder: 
 *      
 */

using UnityEngine;
using System.Collections;

namespace Mx.Res
{

    /// <summary>管理加载资源</summary>
    public class ResoucesMgr : MonoBehaviour
    {
        private Hashtable hashtable;
        private static ResoucesMgr instance;

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static ResoucesMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("_ResoucesMgr").AddComponent<ResoucesMgr>();
                }
                return instance;
            }
        }

        private ResoucesMgr()
        {
            hashtable = new Hashtable();
        }

        private void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// 从Res中加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">路径</param>
        /// <param name="cache">是否缓存</param>
        /// <returns></returns>
        public T Load<T>(string path, bool cache) where T : UnityEngine.Object
        {
            if (hashtable.Contains(path))
            {
                return hashtable[path] as T;
            }

            T assetObj = Resources.Load<T>(path);
            if (assetObj == null)
            {
                Debug.LogWarning("资源不存在 path=" + path);
            }

            if (cache) { hashtable.Add(path, assetObj); }

            return assetObj;
        }

        /// <summary>
        /// 从Res中创建资源
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="cache">是否缓存</param>
        /// <returns></returns>
        public GameObject CreateGameObject(string path, bool cache)
        {
            GameObject assetObj = Load<GameObject>(path, cache);
            GameObject go = Instantiate(assetObj) as GameObject;
            if (go == null) { Debug.LogWarning("从res中加载资源失败，path=" + path); }
            return go;
        }

        /// <summary>
        /// 加载指定路径下的全部文件
        /// </summary>
        /// <typeparam name="T">文件类型</typeparam>
        /// <param name="path">加载资源路径</param>
        /// <param name="cache">是否存入缓存</param>
        /// <returns></returns>
        public T[] LoadAll<T>(string path, bool cache) where T : UnityEngine.Object
        {
            if (hashtable.Contains(path))
            {
                return hashtable[path] as T[];
            }

            T[] assetObj = Resources.LoadAll<T>(path);

            if (assetObj == null)
            {
                Debug.LogError("资源不存在 path=" + path);
            }

            if (cache) {  hashtable.Add(path, assetObj); }

            return assetObj;
        }


    }
}