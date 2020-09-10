/***
 * 
 *    Title: MXFramework
 *           主题: 全局设置
 *    Description: 
 *           功能：
 *                                 
 *    Date: 2019
 *    Version: v1.3.0版本
 *    Modify Recoder: 
 *      
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>全局设置</summary>
public class SettingsWindow : EditorWindow
{
    
    private  List<MacorItem> m_List=new List<MacorItem>();
    private  Dictionary<string, bool> m_Dic = new Dictionary<string, bool>();
    private  string m_macor = null;

    private void OnEnable()
    {
        m_macor = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
     
        m_List.Clear();
        m_List.Add(new MacorItem() { Name = "DEBUG_MODE", DisplayName = "调试模式"});
        m_List.Add(new MacorItem() { Name = "PRINT_LOG", DisplayName = "打印日记"});

        for (int i = 0; i < m_List.Count; i++)
        {
            if (!string.IsNullOrEmpty(m_macor) && m_macor.IndexOf(m_List[i].Name) != -1)
            {
                m_Dic[m_List[i].Name] = true;
            }
            else
            {
                m_Dic[m_List[i].Name] = false;
            }
        }

    }


    private void OnGUI()
    {
        
        for (int i = 0; i < m_List.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");

            m_Dic[m_List[i].Name] = GUILayout.Toggle(m_Dic[m_List[i].Name], m_List[i].DisplayName);

            EditorGUILayout.EndHorizontal();
            
        }

        if (GUILayout.Button("保存", GUILayout.Width(100))) 
        {
            SavaMacor();
        }

    }

    private void SavaMacor()
    {
        m_macor = string.Empty;
        foreach (var item in m_Dic)
        {
            if (item.Value)
            {
                m_macor += string.Format("{0};", item.Key);
            }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, m_macor);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, m_macor);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, m_macor);
    }


    /// <summary>
    /// 宏项目
    /// </summary>
    public class MacorItem
    {
        /// <summary>名称</summary>
        public string Name;
        /// <summary>显示名称</summary>
        public string DisplayName;
    }
}
