/***
 * 
 *    Title: MXFramework
 *           主题: 轻量级的Json文本读写
 *    Description: 
 *           功能：1.Json格式文本数据,增.删.改.查
 *                2.数据加密功能
 *           ps:修改数据时间复杂化比较高，如果要进行大量数据读写建议安装数据库
 *                                  
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder:      
 *
 */

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mx.Util
{
    /// <summary>Json格式文本数据,增.删.改.查</summary>
    public class JsonTextUtil
    {

        #region 数据声明

        public delegate bool DelCondition<T>(T data);

        #endregion

        #region 公开函数

        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="textPath">数据保存文本路径</param>
        /// <param name="data">需要添加的数据</param>
        public static void Add(string textPath, string data)
        {
            WriteData(textPath, data);
        }

        /// <summary>
        ///  添加一条数据
        /// </summary>
        /// <typeparam name="T">泛型类数据（只能是Json可序列化类型）</typeparam>
        /// <param name="textPath">数据保存文本路径</param>
        /// <param name="data">需要保存的数据（只能是Json可序列化类型）</param>
        public static void Add<T>(string textPath, T data)
        {
            string temp = JsonUtility.ToJson(data);
            Add(textPath, temp);
        }

        /// <summary>
        /// 删除指定条件的数据
        /// </summary>
        /// <param name="textPath">数据保存文本路径</param>
        /// <param name="condition">筛选条件</param>
        public static void Delete<T>(string textPath, DelCondition<T> condition)
        {
            DeleteData<T>(textPath, condition);
        }

        /// <summary>
        /// 修改指定条件数据
        /// </summary>
        /// <param name="textPath">数据保存文本路径</param>
        /// <param name="newData">新数据</param>
        /// <param name="condition">条件集合</param>
        /// <param name="isCreate">修改数据不存在的情况下是否创建一条新数据</param>
        public static void Modify<T>(string textPath, T newData, DelCondition<T> condition, bool isCreate = true)
        {
            ModifyData<T>(textPath, newData, condition, isCreate);
        }

        /// <summary>
        /// 查找数据
        /// </summary>
        /// <typeparam name="T">泛型类数据（只能是Json可序列化类型）</typeparam>
        /// <param name="textPath">数据保存文本路径</param>
        /// <param name="condition">条件集合</param>
        /// <returns></returns>
        public static T[] Find<T>(string textPath, DelCondition<T> condition)
        {
            return FindData<T>(textPath, condition);
        }

        /// <summary>
        /// 读取一个表格的数据
        /// </summary>
        /// <typeparam name="T">泛型类数据（只能是Json可序列化类型）</typeparam>
        /// <param name="textPath">数据保存文本路径</param>
        /// <returns></returns>
        public static T ReadText<T>(string textPath)
        {
            return ReadTextData<T>(textPath);
        }

        /// <summary>
        /// 读取一个表格的数据
        /// </summary>
        /// <typeparam name="T">泛型类数据（只能是Json可序列化类型）</typeparam>
        /// <param name="textPath">数据保存文本路径</param>
        /// <returns></returns>
        public static T[] ReadTextToArray<T>(string textPath)
        {
            return ReadTextDataArray<T>(textPath);
        }

        /// <summary>
        /// 删除文本
        /// </summary>
        /// <param name="textPaths"></param>
        public static void DeleteText(params string[] textPaths)
        {
            for (int i = 0; i < textPaths.Length; i++)
            {
                string textPath = textPaths[i];
                if (File.Exists(textPath)) File.Delete(textPath);
            }
        }

        #endregion

        #region 私有函数

        private static void WriteData(string textPath, string data)
        {
            if (string.IsNullOrEmpty(textPath) || string.IsNullOrEmpty(data)) return;

            StreamWriter sw = null;
            if (!File.Exists(textPath)) sw = File.CreateText(textPath);
            else sw = File.AppendText(textPath);

            sw.WriteLine(data + '\n');

            sw.Close();
            sw.Dispose();
        }

        private static T ReadTextData<T>(string textPath)
        {
            T temp;
            StreamReader streamReader = null;

            if (File.Exists(textPath)) streamReader = File.OpenText(textPath);
            else { return default(T); }

            string str = streamReader.ReadToEnd();
            temp = JsonUtility.FromJson<T>(str);

            streamReader.Close();
            streamReader.Dispose();

            return temp;
        }

        private static T[] ReadTextDataArray<T>(string textPath)
        {
            List<T> temp = new List<T>();
            StreamReader streamReader = null;

            if (File.Exists(textPath)) streamReader = File.OpenText(textPath);
            else { return default(T[]); }

            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(str)) { temp.Add(JsonUtility.FromJson<T>(str)); }
            }

            return temp.ToArray();
        }

        private static T[] FindData<T>(string textPath, DelCondition<T> condition)
        {
            if (!File.Exists(textPath)) return default(T[]);

            List<T> tempList = new List<T>();
            T[] tempArr = ReadTextDataArray<T>(textPath);

            for (int i = 0; i < tempArr.Length; i++)
            {
                if (condition(tempArr[i])) { tempList.Add(tempArr[i]); }
            }

            return tempList.ToArray();
        }

        public static void DeleteData<T>(string textPath, DelCondition<T> condition)
        {
            if (!File.Exists(textPath)) return;

            bool isDelete = false;
            string res = string.Empty;
            T[] tempArr = ReadTextDataArray<T>(textPath);

            for (int i = 0; i < tempArr.Length; i++)
            {
                if (!condition(tempArr[i])) { res += JsonUtility.ToJson(tempArr[i]); }
                else isDelete = true;
            }

            if (!isDelete || string.IsNullOrEmpty(res)) return;

            File.Delete(textPath);
            Add(textPath, res);
        }

        private static void ModifyData<T>(string textPath, T newData, DelCondition<T> condition, bool isCreate = true)
        {
            if (!File.Exists(textPath))
            {
                if (isCreate) Add<T>(textPath, newData);
                return;
            }

            bool isModify = false;
            string res = string.Empty;
            T[] tempArr = ReadTextDataArray<T>(textPath);

            for (int i = 0; i < tempArr.Length; i++)
            {
                if (!condition(tempArr[i])) { res += JsonUtility.ToJson(tempArr[i]); }
                else
                {
                    res += JsonUtility.ToJson(newData);
                    isModify = true;
                }
            }

            if (!isModify || string.IsNullOrEmpty(res)) return;
            File.Delete(textPath);
            Add(textPath, res);
        }

        #endregion
    }
}