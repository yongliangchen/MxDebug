using UnityEngine;
using Mx.UI;
using Mx.Msg;
using Mx.Util;
using SuperScrollView;
using Mx.Log;
using UnityEngine.UI;

/// <summary> 全部消息清单 </summary>
public class AllMsgUIForm : BaseUIForm
{
    #region 数据声明

    private LoopListView2 mLoopListView;
    private Scrollbar scrollbar;
    private bool isClickSelection = false;

    #endregion

    #region Unity函数

    private void Awake()
    {
        mLoopListView = UnityHelper.FindTheChildNode(gameObject, "Viewport").GetComponent<LoopListView2>();
        mLoopListView.InitListView(0, OnGetItemByIndex);

        scrollbar = UnityHelper.FindTheChildNode(gameObject, "Scrollbar").GetComponent<Scrollbar>();

        MessageMgr.AddMsgListener("AllMsgUIFormMsg", OnUIFormMessagesEvent);
        MessageMgr.AddMsgListener(Define.GLOBAL_MESSAGE_TYPE, OnGlobalMessagesEvent);

    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener("AllMsgUIFormMsg", OnUIFormMessagesEvent);
        MessageMgr.RemoveMsgListener(Define.GLOBAL_MESSAGE_TYPE, OnGlobalMessagesEvent);
    }

    #endregion


    #region 私有函数

    private void OnUIFormMessagesEvent(string key, object values)
    {
        switch(key)
        {
            case Define.ON_UPDATE_DEBUG_COUNT: OnUpdataDebugCount(JsonUtility.FromJson<DebugCountInfo>((string)values)); break;
            case "CloseData": OnCloseData(); break;
            case Define.ON_ALL_MSG_UI_MOVE_TO_INDEX: mLoopListView.MovePanelToItemIndex((int)values, 0); break;
        }
    }

    private void OnGlobalMessagesEvent(string key, object values)
    {
        switch (key)
        {
            case Define.ON_SELECTION_MSG: isClickSelection = true; ; break;
        }
    }

    private void OnCloseData()
    {
        UserModel.SelectionIndex = -1;
        mLoopListView.SetListItemCount(0, false);
        CloseUIForms(EnumUIFormType.SelectMsgUIForm);
        CloseUIForms(EnumUIFormType.LatestMsgUIForm);
    }

    private LoopListViewItem2 OnGetItemByIndex(LoopListView2 listView, int index)
    {
        LoopListViewItem2 item = listView.NewListViewItem("ItemPrefab1");

        Item itemScript = item.GetComponent<Item>();
        itemScript.SetItemData(index);

        return item;
    }

    private void OnUpdataDebugCount(DebugCountInfo debugCount)
    {
        mLoopListView.SetListItemCount(debugCount.Count, false);
        if (debugCount.Count < 10) scrollbar.value = 0;
        if (scrollbar.value < 0.001f)
        {
            if (!isClickSelection) mLoopListView.MovePanelToItemIndex(debugCount.Count, 0);
        }
        else
        {
            isClickSelection = false;
        }

    }

    #endregion
}

