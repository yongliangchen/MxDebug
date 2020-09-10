using System;
using UnityEngine;

public class Define 
{
    public const string ON_UPDATE_DEBUG_COUNT = "OnUpdateDebugCount";
    public const string ON_ADD_DEBUG_DATA = "OnAddDebugData";

    /// <summary>全局消息类型</summary>
    public const string GLOBAL_MESSAGE_TYPE = "GlobalMessageType";
    /// <summary>选中了调试消息</summary>
    public const string ON_SELECTION_MSG = "SelectionMsg";
    /// <summary>全部消息面板跳转到索引</summary>
    public const string ON_ALL_MSG_UI_MOVE_TO_INDEX = "OnAllMsgUIFormMovePanelToItemIndex";
    /// <summary>添加数据</summary>
    public const string ON_ADDDATA = "OnAddData";
}

[Serializable]
public class DebugCountInfo
{
    public int Count;
    public int LogCount;
    public int WarningCount;
    public int ErrorCount;
}

[Serializable]
public class LatestMsgInfo
{
    public int Index;
    public LogType Type;
    public string Content;
}
