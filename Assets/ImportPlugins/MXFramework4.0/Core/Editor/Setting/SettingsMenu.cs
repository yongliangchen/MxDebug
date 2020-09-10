/***
 * 
 *    Title: MXFramework
 *           主题: 菜单管理
 *    Description: 
 *           功能：
 *                                 
 *    Date: 2019
 *    Version: v1.3.0版本
 *    Modify Recoder: 
 *      
 */

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>设置菜单管理</summary>
public class SettingsMenu
{

    [MenuItem("MXFramework/Settings", false, 20)]
    public static void Settings()
    {
        SettingsWindow window = (SettingsWindow)EditorWindow.GetWindow(typeof(SettingsWindow));
        window.titleContent = new UnityEngine.GUIContent("全局设置");
        window.Show();
    }


}

