using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>窗口管理</summary>
public class WindowManager : MonoBehaviour
{

    private bool isFullScreen = false;

    private void Awake()
    {
        FullScreen();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            isFullScreen = !isFullScreen;
            if (isFullScreen) FullScreen();
            else HalfScreen();
        }
    }

    private void FullScreen()
    {
        if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            isFullScreen = true;
            Screen.fullScreen = true;
            Screen.SetResolution(2880, 1800, true);
        }
    }

    private void HalfScreen()
    {
        if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            isFullScreen = false;
            Screen.fullScreen = false;
            Screen.SetResolution(2880/2, 1800/2, false);
        }
    }
}
