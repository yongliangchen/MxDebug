using UnityEngine;
using Mx.UI;
using Mx.Msg;
using Mx.Util;
using Mx.Log;
using UnityEngine.UI;

/// <summary> 主页UI面板 </summary>
public class MainUIForm : BaseUIForm
{
    #region 数据申明

    private Text text_LogCount;
    private Text text_WarningCount;
    private Text text_ErrorCount;

    private bool isOpenLog = true;
    private bool isOpenWarning = true;
    private bool isOpenError = true;

    private Toggle toggle_Log;
    private Toggle toggle_Warning;
    private Toggle toggle_Error;
    private Toggle toggle_Collapse;

    #endregion

    private void Awake()
    {
        RigisterAllButtonObjectEvent(OnClickButton);
  
        MessageMgr.AddMsgListener("MainUIFormMsg",OnMessagesEvent);

        text_LogCount = UnityHelper.FindTheChildNode(gameObject, "LogCount").GetComponent<Text>();
        text_WarningCount = UnityHelper.FindTheChildNode(gameObject, "WarningCount").GetComponent<Text>();
        text_ErrorCount = UnityHelper.FindTheChildNode(gameObject, "ErrorCount").GetComponent<Text>();
        text_LogCount.text = null;
        text_WarningCount.text = null;
        text_ErrorCount.text = null;

        toggle_Log = UnityHelper.FindTheChildNode(gameObject, "ToggleLog").GetComponent<Toggle>();
        toggle_Warning = UnityHelper.FindTheChildNode(gameObject, "ToggleWarning").GetComponent<Toggle>();
        toggle_Error = UnityHelper.FindTheChildNode(gameObject, "ToggleError").GetComponent<Toggle>();
        toggle_Collapse = UnityHelper.FindTheChildNode(gameObject, "ToggleCollapse").GetComponent<Toggle>();
        toggle_Log.onValueChanged.AddListener(OnChangeLogToggle);
        toggle_Warning.onValueChanged.AddListener(OnChangeWarningToggle);
        toggle_Error.onValueChanged.AddListener(OnChangeErrorToggle);
        toggle_Collapse.onValueChanged.AddListener(OnChangeCollapseToggle);

    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener("MainUIFormMsg", OnMessagesEvent);
    }

    private void OnClickButton(GameObject click)
    {
        switch (click.name)
        {
            case "BtnClose": CloseUIForm(); break;
            case "BtnQiut": Application.Quit(); break;
            case "BtnClear": Clear(); break;
        }
    }

    private void OnMessagesEvent(string key, object values)
    {
        switch (key)
        {
            case Define.ON_ADD_DEBUG_DATA: AddDebugData(JsonUtility.FromJson<DebugData>((string)values)); break;
            case Define.ON_UPDATE_DEBUG_COUNT: OnUpdataDebugCount(JsonUtility.FromJson<DebugCountInfo>((string)values)); break;
        }
    }

    #region 私有函数

    private void Clear()
    {
        SendMessageToUIForm(EnumUIFormType.AllMsgUIForm, "CloseData", "Clear");
        DebugDataManager.Instance.Clear();
        text_LogCount.text = "0";
        text_WarningCount.text = "0";
        text_ErrorCount.text = "0";
    }

    private void AddDebugData(DebugData debugData)
    {
        DebugDataManager.Instance.Add(debugData);
    }

    private void OnUpdataDebugCount(DebugCountInfo debugCount)
    {
        text_LogCount.text = (debugCount.LogCount < 999) ? debugCount.LogCount.ToString() : "999+";
        text_WarningCount.text = (debugCount.WarningCount < 999) ? debugCount.WarningCount.ToString() : "999+";
        text_ErrorCount.text = (debugCount.ErrorCount < 999) ? debugCount.ErrorCount.ToString() : "999+";
    }

    private void OnChangeLogToggle(bool value)
    {
        SendMessageToUIForm(EnumUIFormType.AllMsgUIForm, "CloseData", "OnChangeLogToggle");
        isOpenLog = value;
        DebugDataManager.Instance.OnChangeLogToggle(value);
    }

    private void OnChangeWarningToggle(bool value)
    {
        SendMessageToUIForm(EnumUIFormType.AllMsgUIForm, "CloseData", "OnChangeWarningToggle");
        isOpenWarning = value;
        DebugDataManager.Instance.OnChangeWarningToggle(value);
    }

    private void OnChangeErrorToggle(bool value)
    {
        SendMessageToUIForm(EnumUIFormType.AllMsgUIForm, "CloseData", "OnChangeErrorToggle");
        isOpenError = value;
        DebugDataManager.Instance.OnChangeErrorToggle(value);
    }

    private void OnChangeCollapseToggle(bool value)
    {
        SendMessageToUIForm(EnumUIFormType.AllMsgUIForm, "CloseData", "OnChangeCollapseToggle");
        DebugDataManager.Instance.OnChangeCollapseState((value)? CollapseState.Fold: CollapseState.Expand);
    }

    #endregion
}

