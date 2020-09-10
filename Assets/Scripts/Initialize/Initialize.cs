using UnityEngine;
using Mx.UI;

public class Initialize : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectOfType<WindowManager>() == null) gameObject.AddComponent<WindowManager>();

        UIManager.Instance.OpenUIForms(EnumUIFormType.BackgroundUIForm);
        UIManager.Instance.OpenUIForms(EnumUIFormType.AllMsgUIForm);
        UIManager.Instance.OpenUIForms(EnumUIFormType.MainUIForm);
        UDPServer.Instance.Init();
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.A))
    //    {
    //        Screen.fullScreen = !Screen.fullScreen;
    //    }

    //    if(Input.GetKeyDown(KeyCode.Q))
    //    {
    //        Screen.SetResolution(800, 600, false);
    //    }
    //}
}
