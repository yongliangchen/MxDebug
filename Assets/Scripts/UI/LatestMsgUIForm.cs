using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.UI;
using Mx.Msg;
using Mx.Util;
using UnityEngine.UI;
using Mx.Log;

/// <summary> 最新消息面板 </summary>
public class LatestMsgUIForm : BaseUIForm
{
    private LatestMsgInfo latestMsgInfo=null;
    private Color color;
    private Text text_Content;
    private Image image_Log;
    private Image image_Warning;
    private Image image_Error;

    private void Awake()
    {
        text_Content = UnityHelper.FindTheChildNode(gameObject, "Msg").GetComponent<Text>();
        image_Log = UnityHelper.FindTheChildNode(gameObject, "Log").GetComponent<Image>();
        image_Warning = UnityHelper.FindTheChildNode(gameObject, "Warning").GetComponent<Image>();
        image_Error = UnityHelper.FindTheChildNode(gameObject, "Error").GetComponent<Image>();

        RigisterAllButtonObjectEvent(OnClickButton);
        MessageMgr.AddMsgListener("LatestMsgUIFormMsg",OnMessagesEvent);
    }

    private void OnDisable()
    {
        latestMsgInfo = null;
    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener("LatestMsgUIFormMsg", OnMessagesEvent);
    }

    private void OnClickButton(GameObject click)
    {
        switch(click.name)
        {
            case "BtnClose": CloseUIForm(); break;

            case "BtnClickMsg": OnClickMsg(); break;
        }
    }

    private void OnMessagesEvent(string key, object values)
    {
        switch(key)

        {
            case "LatestMsgInfo":

                latestMsgInfo = JsonUtility.FromJson<LatestMsgInfo>((string)values);
                AddMsg();
                break;
        }
    }

    private void AddMsg()
    {
        if (latestMsgInfo == null) return;

        text_Content.text = latestMsgInfo.Content;
        image_Log.gameObject.SetActive(latestMsgInfo.Type == LogType.Assert || latestMsgInfo.Type == LogType.Log);
        image_Warning.gameObject.SetActive(latestMsgInfo.Type == LogType.Warning);
        image_Error.gameObject.SetActive(latestMsgInfo.Type == LogType.Exception || latestMsgInfo.Type == LogType.Error);

        switch (latestMsgInfo.Type)
        {
            case LogType.Assert:
            case LogType.Log:

                color = Color.white;
                break;

            case LogType.Warning:

                color = Color.yellow;
                break;

            default:

                color = Color.red;
                break;
        }

        text_Content.color = color;
    }

    private void OnClickMsg()
    {
        if (latestMsgInfo == null) return;

        DebugData debugData=DebugDataManager.Instance.GetDataByIndex(latestMsgInfo.Index);
        if (debugData == null) return;

        UserModel.SelectionIndex = latestMsgInfo.Index;
        UIManager.Instance.OpenUIForms(EnumUIFormType.SelectMsgUIForm);

        MessageMgr.SendMessageToUIForm(EnumUIFormType.SelectMsgUIForm, "Info", debugData.Condition + "\n" + debugData.StackTrace);
        MessageMgr.SendMessageToUIForm(EnumUIFormType.AllMsgUIForm, Define.ON_ALL_MSG_UI_MOVE_TO_INDEX, latestMsgInfo.Index);
        MessageMgr.SendMessage(Define.GLOBAL_MESSAGE_TYPE, Define.ON_SELECTION_MSG, debugData.ID);
    }
}

