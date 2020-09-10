using System.Collections.Generic;
using UnityEngine;
using Mx.Log;
using Mx.Util;
using System.Linq;
using Mx.Msg;
using Mx.UI;

public class DebugDataManager : MonoSingleton<DebugDataManager>
{
    private Dictionary<string, DebugData> dicFoldAllData = new Dictionary<string, DebugData>();
    private Dictionary<string, DebugData> dicExpandAllData = new Dictionary<string, DebugData>();
    private Dictionary<string, DebugData> dicCurrentData = new Dictionary<string, DebugData>();
    private CollapseState collapseState = CollapseState.Fold;
    private int logCount = 0;
    private int warningCount = 0;
    private int errorCount = 0;

    private bool isOpenLog = true;
    private bool isOpenWarning = true;
    private bool isOpenError = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Add(DebugData debugData)
    {
        string expandKey = debugData.ID;
        string foldKey = debugData.StackTrace+ debugData.Condition;

        dicExpandAllData.Add(expandKey, debugData);

        if (!dicFoldAllData.ContainsKey(foldKey))
        {
            DebugData newData = new DebugData();
            newData.Condition = debugData.Condition;
            newData.Count = 1;
            newData.StackTrace = debugData.StackTrace;
            newData.Tiem = debugData.Tiem;
            newData.Type = debugData.Type;

            dicFoldAllData.Add(foldKey, newData);
        }
        else { dicFoldAllData[foldKey].Count++; }

        switch (collapseState)
        {
            case CollapseState.Fold: AddCurrentData(foldKey, debugData); break;
            case CollapseState.Expand: AddCurrentData(expandKey, debugData); break;
        }

        UpdataDebugCount();
    }

    public void Clear()
    {
        dicExpandAllData.Clear();
        dicFoldAllData.Clear();
        dicCurrentData.Clear();
        logCount = 0;
        warningCount = 0;
        errorCount = 0;
    }

    public DebugData GetDataByIndex(int index)
    {
        if (index >= dicCurrentData.Count) return null;
        return dicCurrentData.Values.ToArray()[index];
    }

    public int GetIndexByKey(string key)
    {
        string[] keys = dicCurrentData.Keys.ToArray();
        for (int i=0;i< keys.Length;i++)
        {
            if (key.Equals(keys[i])) return i;
        }

        return -1;
    }

    public int GetCount()
    {
        return dicCurrentData.Count;
    }

    public void OnChangeCollapseState(CollapseState collapseState)
    {
        this.collapseState = collapseState;
        UserModel.CollapseState = collapseState;
        UpdateData();
    }

    public void OnChangeLogToggle(bool value)
    {
        isOpenLog = value;
        UpdateData();
    }

    public void OnChangeWarningToggle(bool value)
    {
        isOpenWarning = value;
        UpdateData();
    }

    public void OnChangeErrorToggle(bool value)
    {
        isOpenError = value;
        UpdateData();
    }

    #region 私有函数

    private void AddCurrentData(string key, DebugData debugData)
    {
        switch (collapseState)
        {
            case CollapseState.Fold:

                if (!dicCurrentData.ContainsKey(key))
                {
                    AddCount(debugData.Type);

                    if (IsAddCurrentData(debugData.Type))
                    {
                        dicCurrentData.Add(key, debugData);
                        AddLatestMsg(key, debugData.Condition, debugData.Type);
                    }
                }
                else
                {
                    if (IsAddCurrentData(debugData.Type))
                    {
                        dicCurrentData[key].Count++;
                        AddLatestMsg(key, debugData.Condition, debugData.Type);
                    }
                }
                break;

            case CollapseState.Expand:

                AddCount(debugData.Type);

                if (IsAddCurrentData(debugData.Type))
                {
                    dicCurrentData.Add(key, debugData);
                    AddLatestMsg(key, debugData.Condition, debugData.Type);
                }
                break;
        }

        MessageMgr.SendMessage(Define.GLOBAL_MESSAGE_TYPE, Define.ON_ADDDATA, key);
    }

    private void AddCount(LogType type)
    {
        if (type == LogType.Assert || type == LogType.Log) logCount++;
        else if (type == LogType.Warning) warningCount++;
        else errorCount++;
    }

    private bool IsAddCurrentData(LogType type)
    {
        if ((type == LogType.Assert || type == LogType.Log) && !isOpenLog) return false;
        if (type == LogType.Warning && !isOpenWarning) return false;
        if ((type == LogType.Exception || type == LogType.Error) && !isOpenError) return false;

        return true;
    }

    private void AddLatestMsg(string key, string Content, LogType type)
    {
        int tempIndex = GetIndexByKey(key);
        if (tempIndex == -1) return;

        LatestMsgInfo latestMsgInfo = new LatestMsgInfo();
        latestMsgInfo.Index = tempIndex;
        latestMsgInfo.Type = type;
        latestMsgInfo.Content = Content;

        UIManager.Instance.OpenUIForms(EnumUIFormType.LatestMsgUIForm);
        MessageMgr.SendMessageToUIForm(EnumUIFormType.LatestMsgUIForm, "LatestMsgInfo", JsonUtility.ToJson(latestMsgInfo));
    }

    private void UpdateData()
    {
        logCount = 0;
        warningCount = 0;
        errorCount = 0;
        dicCurrentData.Clear();

        switch (collapseState)
        {
            case CollapseState.Fold:

                foreach (string key in dicFoldAllData.Keys)
                {
                    AddCurrentData(key, dicFoldAllData[key]);
                }
                break;

            case CollapseState.Expand:

                foreach (string key in dicExpandAllData.Keys)
                {
                    AddCurrentData(key, dicExpandAllData[key]);
                }
                break;
        }

        UpdataDebugCount();
    }


    private void UpdataDebugCount()
    {
        DebugCountInfo debugCount = new DebugCountInfo();
        debugCount.Count = dicCurrentData.Count;
        debugCount.LogCount = logCount;
        debugCount.WarningCount = warningCount;
        debugCount.ErrorCount = errorCount;
        string msg = JsonUtility.ToJson(debugCount);

        MessageMgr.SendMessageToUIForm(EnumUIFormType.MainUIForm, Define.ON_UPDATE_DEBUG_COUNT, msg);
        MessageMgr.SendMessageToUIForm(EnumUIFormType.AllMsgUIForm, Define.ON_UPDATE_DEBUG_COUNT, msg);
    }

    #endregion
}

/// <summary>折叠状态</summary>
public enum CollapseState
{
    /// <summary>折叠</summary>
    Fold,
    /// <summary>展开</summary>
    Expand,
}
