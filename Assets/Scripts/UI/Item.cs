using Mx.Log;
using UnityEngine;
using UnityEngine.UI;
using Mx.Msg;
using Mx.UI;

public class Item : MonoBehaviour
{
    private GameObject go_Count;
    private Text text_Content;
    private Text text_Count;
    private Image image_Log;
    private Image image_Warning;
    private Image image_Error;
    private Button button;
    private int index = 0;
    private GameObject go_Selection;
    private DebugData debugData;

    private void Awake()
    {
        go_Count = transform.Find("Count").gameObject;
        go_Selection = transform.Find("Selection").gameObject;

        button = transform.GetComponent<Button>();
        if(button==null)
        {
            button = gameObject.AddComponent<Button>();
        }

        button.onClick.AddListener(OnClickButton);

        text_Content = transform.Find("Content").GetComponent<Text>();
        text_Count= transform.Find("Count/Text").GetComponent<Text>();
        text_Count.text = null;
        text_Content.text = null;

        image_Log= transform.Find("Log").GetComponent<Image>();
        image_Warning = transform.Find("Warning").GetComponent<Image>();
        image_Error = transform.Find("Error").GetComponent<Image>();

        MessageMgr.AddMsgListener(Define.GLOBAL_MESSAGE_TYPE, OnGlobalMessagesEvent);
    }

    private void Update()
    {
        if(debugData!=null) OnSelectionMsg();
    }

    private void OnDestroy()
    {
        MessageMgr.RemoveMsgListener(Define.GLOBAL_MESSAGE_TYPE, OnGlobalMessagesEvent);
    }

    public void SetItemData(int index)
    {
        this.index = index;
        GetDataByIndex(index);
    }

    public void GetDataByIndex(int index)
    {
        debugData = DebugDataManager.Instance.GetDataByIndex(index);
        UpdateData();
    }

    private void UpdateData()
    {
        if (debugData == null) return;

        go_Count.SetActive(UserModel.CollapseState == CollapseState.Fold);
        image_Log.gameObject.SetActive(debugData.Type == LogType.Assert || debugData.Type == LogType.Log);
        image_Warning.gameObject.SetActive(debugData.Type == LogType.Warning);
        image_Error.gameObject.SetActive(debugData.Type == LogType.Exception || debugData.Type == LogType.Error);
        text_Content.text = debugData.Condition;
        text_Count.text = (debugData.Count > 9999) ? "9999+" : debugData.Count.ToString();
    }

    private void OnClickButton()
    {
        UserModel.SelectionIndex = index;
        MessageMgr.SendMessage(Define.GLOBAL_MESSAGE_TYPE, Define.ON_SELECTION_MSG, debugData.ID);
        UIManager.Instance.OpenUIForms(EnumUIFormType.SelectMsgUIForm);
        MessageMgr.SendMessageToUIForm(EnumUIFormType.SelectMsgUIForm, "Info", debugData.Condition+"\n"+debugData.StackTrace);
    }

    private void OnGlobalMessagesEvent(string key, object values)
    {
        switch (key)
        {
            case Define.ON_ADDDATA: UpdateData(); break;
        }
    }

    private void OnSelectionMsg()
    {
        if (UserModel.SelectionIndex < 0) go_Selection.SetActive(false);
        else
        {
            go_Selection.SetActive(UserModel.SelectionIndex == index);
        }
       
    }
}
