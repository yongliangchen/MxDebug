using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mx.UI;
using Mx.Msg;
using Mx.Util;

/// <summary> 背景UI面板 </summary>
public class BackgroundUIForm : BaseUIForm
{
    private void Awake()
    {
        RigisterAllButtonObjectEvent(OnClickButton);
        MessageMgr.AddMsgListener("BackgroundUIFormMsg",OnMessagesEvent);
    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener("BackgroundUIFormMsg", OnMessagesEvent);
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
        
    }
}

