using Mx.Msg;
using Mx.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Mx.UI
{
    /// <summary>UI的父类</summary>
    public abstract class BaseUIForm : MonoBehaviour
    {
        #region 封装子类常用的方法

        /// <summary>
        /// 注册按钮事件
        /// </summary>
        /// <param name="buttonName">按钮的名称</param>
        /// <param name="delHandle">委托的方法</param>
        protected void RigisterButtonObjectEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle)
        {
            GameObject goButton = UnityHelper.FindTheChildNode(this.gameObject, buttonName).gameObject;
            if (goButton != null) { EventTriggerListener.Get(goButton).onClick = delHandle; }
            else
            {
               Debug.LogWarning(GetType() + "/RigisterButtonObjectEvent/add button event is error! button is null!  buttonName:" + buttonName);
            }
        }

        /// <summary>
        /// 注册所有的按钮事件(给子节点中所有带有Button组建的对象添加按钮事件)
        /// </summary>
        /// <param name="delHandle">Del handle.</param>
        protected void RigisterAllButtonObjectEvent(EventTriggerListener.VoidDelegate delHandle)
        {
            //添加按钮点击事件
            Button[] buttonArr = this.GetComponentsInChildren<Button>(true);
            for (int i = 0, len = buttonArr.Length; i < len; i++)
            {
                GameObject button = buttonArr[i].gameObject;
                EventTriggerListener.Get(button).onClick = delHandle;
            }
        }

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormNames">需要打开的窗体名字</param>
        protected void OpenUIForms(params string[] uiFormNames)
        {
            UIManager.Instance.OpenUIForms(uiFormNames);
        }

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="uiFormTypes">需要打开的窗体类型数组</param>
        protected void OpenUIForms(params EnumUIFormType[] uiFormTypes)
        {
            UIManager.Instance.OpenUIForms(uiFormTypes);
        }

        /// <summary>关闭当前UI窗体</summary>
        protected void CloseUIForm()
        {
            string[] tempStringArr = GetType().ToString().Split('.');
            string struiFormName = tempStringArr[tempStringArr.Length - 1];
            UIManager.Instance.CloseUIForms(struiFormName);
        }

        /// <summary>
        /// 关闭UI窗体
        /// </summary>
        /// <param name="uiFormNames">需要关闭的窗体名字数组</param>
        protected void CloseUIForms(params string[] uiFormNames)
        {
            UIManager.Instance.CloseUIForms(uiFormNames);
        }

        /// <summary>
        /// 关闭UI窗体
        /// </summary>
        /// <param name="uiFormTypes">需要关闭窗体类型数组</param>
        protected void CloseUIForms(params EnumUIFormType[] uiFormTypes)
        {
            UIManager.Instance.CloseUIForms(uiFormTypes);
        }

        /// <summary>
        /// 发送消息给UI窗体
        /// </summary>
        /// <param name="uIFormType">接收消息的UI窗体</param>
        /// <param name="key">消息名称</param>
        /// <param name="values">消息内容</param>
        protected void SendMessageToUIForm(EnumUIFormType uIFormType, string key, object values)
        {
            MessageCenter.SendMessage(uIFormType.ToString() + "Msg", key, values);
        }
    }

    #endregion
}