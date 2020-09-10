/***
 * 
 *    Title: MXFramework
 *           主题: Unity的帮助类
 *    Description: 
 *           功能：
 *                                  
 *    Date: 2020
 *    Version: 4.0版本
 *    Modify Recoder:      
 *
 */

using UnityEngine;

namespace Mx.Util
{
    public class UnityHelper : MonoBehaviour
    {
        /// <summary>
        /// 查找子节点对象(递归算法)
        /// </summary>
        /// <returns>The the child node.</returns>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">查找的子节点名称</param>
        public static Transform FindTheChildNode(GameObject goParent, string childName)
        {
            Transform secrchTrans = null;

            secrchTrans = goParent.transform.Find(childName);
            if (secrchTrans == null)
            {
                foreach (Transform trans in goParent.transform)
                {
                    secrchTrans = FindTheChildNode(trans.gameObject, childName);
                    if (secrchTrans != null)
                    {
                        return secrchTrans;
                    }
                }
            }

            return secrchTrans;
        }

        /// <summary>
        /// 查找子节点中的脚本
        /// </summary>
        /// <returns> 泛型 </returns>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">子对象名称</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetTheChildNodeComponetScripts<T>(GameObject goParent, string childName) where T : Component
        {
            Transform searchTranformNode = null;

            searchTranformNode = FindTheChildNode(goParent, childName);
            if (searchTranformNode != null)
            {
                return searchTranformNode.gameObject.GetComponent<T>();
            }
            else
            {
                Debug.LogWarning("GetTheChildNodeComponetScripts is error! childName:" + childName);
                return null;
            }
        }

        /// <summary>
        /// 给子节点添加脚本
        /// </summary>
        /// <returns> 泛型 </returns>
        /// <param name="goParent">父对象</param>
        /// <param name="childName">子对象名称</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T AddChildNodeCompnent<T>(GameObject goParent, string childName) where T : Component
        {
            Transform searchTranformNode = null;

            searchTranformNode = FindTheChildNode(goParent, childName);
            if (searchTranformNode != null)
            {
                T[] componentScriptsArray = searchTranformNode.GetComponents<T>();
                for (int i = 0; i < componentScriptsArray.Length; i++)
                {
                    Destroy(componentScriptsArray[i]);
                }

                return searchTranformNode.gameObject.AddComponent<T>();
            }
            else
            {
                Debug.LogWarning("AddChildNodeCompnent is error! childName:" + childName);
                return null;
            }
        }

        /// <summary>
        /// 将子节点添加到父对象
        /// </summary>
        /// <param name="parents">父对象</param>
        /// <param name="child">子对象</param>
        public static void AddChildNodeToParentNode(Transform parents, Transform child)
        {
            child.SetParent(parents, false);
            child.localPosition = Vector3.zero;
            child.localScale = Vector3.one;
            child.localEulerAngles = Vector3.zero;
        }


    }//class_end
}
