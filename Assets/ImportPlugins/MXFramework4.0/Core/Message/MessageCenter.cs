/***
 * 
 *    Title: MXFramework
 *           主题: 消息中心
 *    Description: 
 *           功能：负责消息的收发
 *                                  
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder:      
 *
 */

using System.Collections.Generic;
using UnityEngine;
using System;

namespace Mx.Msg
{
    /// <summary>消息中心</summary>
    public class MessageCenter : MonoBehaviour
    {
        /// <summary>消息中心缓存集合</summary>
        public static Dictionary<string, Action<string, object>> dicMessages = new Dictionary<string, Action<string, object>>();

        /// <summary>
        /// 添加消息的监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public static void AddMsgListener(string messageType, Action<string, object> handler)
        {
            if(!dicMessages.ContainsKey(messageType))
            {
                dicMessages.Add(messageType, null);
            }
            dicMessages[messageType] += handler;
        }

        /// <summary>
        /// 取消消息监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public static void RemoveMsgListener(string messageType, Action<string, object> handler)
        {
            if(dicMessages.ContainsKey(messageType))
            {
                dicMessages[messageType] -= handler;
            }
        }

        /// <summary>
        /// 取消所有的消息监听
        /// </summary>
        public static void ClearAllMsgListener()
        {
            if(dicMessages!=null)
            {
                dicMessages.Clear();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="key">消息名称</param>
        /// <param name="values">消息体</param>
        public static void SendMessage(string messageType,string key,object values)
        {
            Action<string, object> del;
            dicMessages.TryGetValue(messageType, out del);
            if(del!=null)
            {
                del(key,values);
            }
        }

    }
}