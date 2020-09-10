/***
 * 
 *    Title: MXFramework
 *           主题: UI模块全局定义
 *    Description: 
 *           功能：1.UI模块全局枚举定义
 *                2.UI模块全局委托定义
 *                3.UI模块全局数据定义 
 *               
 *                                  
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder:      
 *
 */

using UnityEngine;
using System.IO;

namespace Mx.UI
{
    public sealed class UIDefine
    {
        /// <summary>UIRoot的路径</summary>
        public const string PATH_UIROOT = "UIRoot";
        /// <summary>普通窗体节点常量</summary>
        public const string NORMAL_MODE = "Normal";
        /// <summary>固定窗体节点常量</summary>
        public const string FIXED_MODE = "Fixed";
        /// <summary>弹出窗体节点常量</summary>
        public const string POPUP_MODE = "PopUp";
        /// <summary>通知窗体节点常量</summary>
        public const string NOTICE_MODE = "Notice";
        /// <summary>脚本管理节点常量</summary>
        public const string SCRIPTSLMANAGER_MODE = "ScriptsManager";

        private static string uiFormCSharpScriptsPath = Application.dataPath + "/Scripts/UI/";
        /// <summary>Ui窗口C#脚本存放路径</summary>
        public static string UIFormCSharpScriptsPath
        {
            get
            {
                if (!Directory.Exists(uiFormCSharpScriptsPath)) Directory.CreateDirectory(uiFormCSharpScriptsPath);
                return uiFormCSharpScriptsPath;
            }
        }

        public const string Template_UIFORM_NAMES= "Template/UI/Template_UIFormNames";
        public const string Template_UIFORM_CSHARP_BASE = "Template/UI/Template_UIFormCSharpBase";
    }

    #region 系统枚举类型

    /// <summary>Ui窗体层级</summary>
    public enum UIFormDepth
    {
        /// <summary>普通窗体</summary>
        Normal,
        /// <summary>固定窗体</summary>
        Fixed,
        /// <summary>弹出窗体</summary>
        PopUp,
        /// <summary>通知面板</summary>
        Notice,
    }

    /// <summary>UI窗体显示模式</summary>
    public enum UIFormShowMode
    {
        /// <summary>普通窗体模式</summary>
        Normal,
        /// <summary>反向切换模式(ps只能按照顺序打开和关闭不然就会出错)</summary>
        ReverseChange,
        /// <summary>隐藏其他模式</summary>
        HideOther,
    }

    /// <summary>UI窗体状态</summary>
    public enum UIFormState
    {
        /// <summary>未知状态</summary>
        None,
        /// <summary>加载当中</summary>
        Loading,
        /// <summary>打开状态</summary>
        Open,
        /// <summary>关闭</summary>
        Close,
        /// <summary>隐藏状态</summary>
        Hiding,
        /// <summary>禁用</summary>
        Disable,
        /// <summary>释放</summary>
        //Release,
        /// <summary>发生错误</summary>
        Error,
    }

    /// <summary>UI加载方式</summary>
    public enum LoadType
    {
        /// <summary>通过Resources方式加载</summary>
        Resources,
        /// <summary>通过AssetBundle方式加载</summary>
        AssetBundle,
    }

    #endregion


}


