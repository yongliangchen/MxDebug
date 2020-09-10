using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.UI;
using Mx.Msg;
using Mx.Util;
using UnityEngine.UI;

/// <summary> 选中消息UI面板 </summary>
public class SelectMsgUIForm : BaseUIForm
{
    private string content;
    private Text text_Msg;
    private InputField inputField;
    private ScrollRect scrollRect;

    private void Awake()
    {
        scrollRect= UnityHelper.FindTheChildNode(gameObject, "ScrollRect").GetComponent<ScrollRect>();
        scrollRect.enabled = false;
        text_Msg = UnityHelper.FindTheChildNode(gameObject, "Msg").GetComponent<Text>();
        inputField = UnityHelper.FindTheChildNode(gameObject, "InputField").GetComponent<InputField>();
        RigisterAllButtonObjectEvent(OnClickButton);
        MessageMgr.AddMsgListener("SelectMsgUIFormMsg",OnMessagesEvent);
        inputField.onValueChanged.AddListener(Change);
        inputField.onEndEdit.AddListener(End);
    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener("SelectMsgUIFormMsg", OnMessagesEvent);
    }

    private void OnClickButton(GameObject click)
    {
        switch(click.name)
        {
            case "BtnClose": CloseUIForm(); break;
        }
    }

    private void OnMessagesEvent(string key, object values)
    {
        switch (key)
        {
            case "Info":

                content = (string)values;
                SetInfo(content);
                break;
        }
    }

    private void Change(string value)
    {
        SetInfo(content);
    }

    private void End(string value)
    {
        SetInfo(content);
    }

    private void SetInfo(string info)
    {
        text_Msg.text = info;
        inputField.text = info;
        scrollRect.enabled = text_Msg.GetComponent<RectTransform>().sizeDelta.y>0;
    }
}

