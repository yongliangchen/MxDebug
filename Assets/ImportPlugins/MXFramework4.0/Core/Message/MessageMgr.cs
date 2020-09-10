using System;
using UnityEngine;

namespace Mx.Msg
{
    /// <summary>消息管理</summary>
    public class MessageMgr : MonoBehaviour
    {
        /// <summary>
        /// 添加消息的监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public static void AddMsgListener(string messageType, Action<string, object> handler)
        {
            MessageCenter.AddMsgListener(messageType, handler);
        }

        /// <summary>
        /// 取消消息监听
        /// </summary>
        /// <param name="messageType">消息分类</param>
        /// <param name="handler">消息委托</param>
        public static void RemoveMsgListener(string messageType, Action<string, object> handler)
        {
            MessageCenter.RemoveMsgListener(messageType, handler);
        }

        /// <summary>
        /// 取消所有的消息监听
        /// </summary>
        public static void ClearAllMsgListener()
        {
            MessageCenter.ClearAllMsgListener();
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageType">消息类型</param>
        /// <param name="key">消息名称</param>
        /// <param name="values">消息体</param>
        public static void SendMessage(string messageType, string key, object values)
        {
            MessageCenter.SendMessage(messageType, key, values);
        }

        /// <summary>
        /// 发送给指定UI消息
        /// </summary>
        /// <param name="uIFormType">接收消息UI面板</param>
        /// <param name="key">消息名称</param>
        /// <param name="values">消息体</param>
        public static void SendMessageToUIForm(EnumUIFormType uIFormType, string key, object values)
        {
            MessageCenter.SendMessage(uIFormType.ToString() + "Msg", key, values);
        }
    }
}