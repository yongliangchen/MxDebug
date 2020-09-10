using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mx.Res
{

    /// <summary>
    /// 管理引用关系和依赖关系
    /// </summary>
    public class ABRelating
    {
        /// <summary>AssetBundle名字</summary>
        //private string _ABName;
        /// <summary>所有的依赖包集合</summary>
        private List<string> _LisAllDependenceAB;
        /// <summary>本包中所有的引用包集合</summary>
        private List<string> _LisAllReferenceAB;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ABRelating(string abName)
        {
            //if (!string.IsNullOrEmpty(abName))
            //{
            //    _ABName = abName;
            //}

            _LisAllDependenceAB = new List<string>();
            _LisAllReferenceAB = new List<string>();

        }

        #region 依赖关系

        /// <summary>
        /// 添加依赖关系
        /// </summary>
        /// <param name="abName">添加依赖项AssetBundle名字</param>
        public void AddDependence(string abName)
        {
            if (!_LisAllDependenceAB.Contains(abName))
            {
                _LisAllDependenceAB.Add(abName);
            }
        }


        /// <summary>
        /// 移除依赖关系
        /// </summary>
        /// <param name="abName">移除AssetBundle包名称</param>
        /// true:此 AssetBundle 没有依赖项
        /// false:此 AssetBundle 还有其他依赖项
        public bool RemoveDependence(string abName)
        {
            if (_LisAllDependenceAB.Contains(abName))
            {
                _LisAllDependenceAB.Remove(abName);
            }
            if (_LisAllDependenceAB.Count > 0)
            {
                return false;
            }

            else
            {
                return true;
            }
        }


        /// <summary>
        /// 获取所有的依赖关系
        /// </summary>
        /// <returns>The all dependenc.</returns>
        public List<string> GetAllDependenc()
        {
            return _LisAllDependenceAB;
        }

        #endregion


        #region 引用关系

        /// <summary>
        /// 添加引用关系
        /// </summary>
        /// <param name="abName">添加引用关系AssetBundle名字</param>
        public void AddReference(string abName)
        {
            if (!_LisAllReferenceAB.Contains(abName))
            {
                _LisAllReferenceAB.Add(abName);
            }
        }


        /// <summary>
        /// 移除引用关系
        /// </summary>
        /// <param name="abName">移除AssetBundle包名称</param>
        /// true:此 AssetBundle 没有引用项
        /// false:此 AssetBundle 还有其他引用项
        public bool RemoveReference(string abName)
        {
            if (_LisAllReferenceAB.Contains(abName))
            {
                _LisAllReferenceAB.Remove(abName);
            }
            if (_LisAllReferenceAB.Count > 0)
            {
                return false;
            }

            else
            {
                return true;
            }
        }


        /// <summary>
        /// 获取所有的引用关系
        /// </summary>
        /// <returns>The all dependenc.</returns>
        public List<string> GetAllReference()
        {
            return _LisAllReferenceAB;
        }

        #endregion


    }
}

