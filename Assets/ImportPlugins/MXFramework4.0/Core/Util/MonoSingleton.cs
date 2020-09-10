/***
 * 
 *    Title: MXFramework
 *           主题: 脚本单例类
 *    Description: 
 *           功能：实现单利类
 *                                  
 *    Date: 2020
 *    Version: 4.0版本
 *    Modify Recoder:      
 *
 */

using UnityEngine;

namespace Mx.Util
{
    /// <summary>脚本单例类</summary>
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance == null) { instance = new GameObject("_"+typeof(T).Name).AddComponent<T>(); }
                    instance.Init();//相当于构造函数
                }
                return instance;
            }
        }

        public virtual void Init()
        {

        }

    }
}
