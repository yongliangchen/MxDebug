/***
 * 
 *    Title: MXFramework
 *           主题: UI管理器
 *    Description: 
 *           功能：是整个UI框架的核心。
 *                                  
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder:      
 *
 */

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Mx.Util;
using Mx.Config;
using Mx.Res;
using System;

namespace Mx.UI
{
    /// <summary>UI管理器</summary>
    public class UIManager : MonoSingleton<UIManager>
    {
        #region 私有函数

        private UIConfigDatabase uIConfig;

        /// <summary>缓存中所有的UI窗体</summary>
        private Dictionary<string, UIFormItem> dicAllUIForms = new Dictionary<string, UIFormItem>();
        /// <summary>当前显示的UI窗体</summary>
        private Dictionary<string, UIFormItem> dicOpenUIForms = new Dictionary<string, UIFormItem>();
        /// <summary>UI状态</summary>
        private Dictionary<string, UIFormState> dicUIFormState = new Dictionary<string, UIFormState>();
        /// <summary>定义“栈”集合，储存显示当前所有【反向切换】的窗体集合</summary>
        private Stack<string> staCurrentUIForms = new Stack<string>();

        /// <summary>UI根节点</summary>
        private Transform traUiRoot = null;
        /// <summary>全屏幕显示的节点</summary>
        private Transform traNormal = null;
        /// <summary>固定显示节点</summary>
        private Transform traFixed = null;
        /// <summary>弹出节点</summary>
        private Transform traPopUp = null;
        /// <summary>通知面板</summary>
        private Transform traNotice = null;
        /// <summary>UI管理脚本节点</summary>
        private Transform traScripts = null;

        #endregion

        #region Unity函数

        private void Awake()
        {
            uIConfig = DataManager.Instance.GetDatabase<UIConfigDatabase>();

            InitState();
            InitUIRoot();

            traNormal = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.NORMAL_MODE);
            traFixed = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.FIXED_MODE);
            traPopUp = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.POPUP_MODE);
            traNotice = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.NOTICE_MODE);
            traScripts = UnityHelper.FindTheChildNode(traUiRoot.gameObject, UIDefine.SCRIPTSLMANAGER_MODE);

            this.gameObject.transform.SetParent(traScripts, false);
            DontDestroyOnLoad(traUiRoot);//加载场景的时候不销毁
        }

        #endregion

        #region 公开函数

        public void OpenUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];

                if (dicOpenUIForms.ContainsKey(uiFormName)) continue;

                UIConfigData uiInfo = uIConfig.GetDataByKey(uiFormName);
                if (uiInfo == null)
                {
                    Debug.LogWarning(GetType() + "/OpenUIForms()/ Open ui error! ui is null! uiFormName:" + uiFormName);
                    return;
                }

                if (dicAllUIForms.ContainsKey(uiFormName))
                {
                    ChangeUIFormState(uiFormName, UIFormState.Open);
                    if (dicAllUIForms[uiFormName] != null) dicAllUIForms[uiFormName].gameObject.SetActive(true);
                    if (dicAllUIForms[uiFormName] != null) dicOpenUIForms.Add(uiFormName, dicAllUIForms[uiFormName]);
                }
                else
                {
                    dicAllUIForms.Add(uiFormName, null);
                    ChangeUIFormState(uiFormName, UIFormState.Loading);
                    LoadUIForm(uiInfo, uiFormName);
                }
            }
        }

        public void OpenUIForms(params EnumUIFormType[] uiFormTypes)
        {
            for (int i = 0; i < uiFormTypes.Length; i++)
            {
                OpenUIForms(uiFormTypes[i].ToString());
            }
        }
      
        public void CloseUIForms(params string[] uiFormNames)
        {
            for (int i = 0; i < uiFormNames.Length; i++)
            {
                string uiFormName = uiFormNames[i];
              
                if (dicOpenUIForms.ContainsKey(uiFormName))
                {
                    if (dicOpenUIForms[uiFormName] != null) dicOpenUIForms[uiFormName].gameObject.SetActive(false);
                    dicOpenUIForms.Remove(uiFormName);
                    ChangeUIFormState(uiFormName, UIFormState.Close);
                }
                else
                {
                    string msg = string.Empty;
                    if (!dicUIFormState.ContainsKey(uiFormName))
                    {
                        msg = GetType() + "/CloseUIForms()/ Close ui error! ui is null! uiFormName:" + uiFormName;
                    }
                    else
                    {
                        ChangeUIFormState(uiFormName, UIFormState.Close);
                        UIFormState uIFormState = dicUIFormState[uiFormName];
                        msg = string.Format(GetType() + "/CloseUIForms()/ Close ui error! currentState:{0} uiFormName:{1}", uIFormState, uiFormName);
                    }
                    Debug.LogWarning(msg);
                }
            }
        }

        public void CloseUIForms(params EnumUIFormType[] uiFormTypes)
        {
            for (int i = 0; i < uiFormTypes.Length; i++)
            {
                CloseUIForms(uiFormTypes[i].ToString());
            }
        }

        public void CloseAllUIForms()
        {
            if (dicOpenUIForms == null || dicOpenUIForms.Count < 1) return;

            string[] openUIForms = dicOpenUIForms.Keys.ToArray<string>();
            for (int i = 0; i < openUIForms.Length; i++)
            {
                CloseUIForms(openUIForms[i]);
                ChangeUIFormState(openUIForms[i], UIFormState.Close);
            }
        }

        public void ClearUIFormsStack()
        {
            ClearStack();
        }

        #endregion

        #region 私有函数

        private void InitUIRoot()
        {
            traUiRoot = Mx.Res.ResoucesMgr.Instance.CreateGameObject(UIDefine.PATH_UIROOT, false).transform;
            traUiRoot.name = "UIRoot";
        }

        private void InitState()
        {
            dicUIFormState.Clear();
            for (int i = 0; i < uIConfig.GetCount(); i++)
            {
                UIConfigData uiInfo = uIConfig.GetAllData()[i];
                if (!dicUIFormState.ContainsKey(uiInfo.Name)) ChangeUIFormState(uiInfo.Name, UIFormState.None);
            }
        }

        private void LoadUIForm(UIConfigData uiInfo, string uiFormName)
        {
            Transform parent = null;
            switch ((UIFormDepth)uiInfo.UIFormsDepth)
            {
                case UIFormDepth.Normal: parent = traNormal; break;
                case UIFormDepth.Fixed: parent = traFixed; break;
                case UIFormDepth.PopUp: parent = traPopUp; break;
                case UIFormDepth.Notice: parent = traNotice; break;
            }

            UIParam uiParam = new UIParam();
            uiParam.UIFormsDepth = (UIFormDepth)uiInfo.UIFormsDepth;
            uiParam.UIFormsShowMode = (UIFormShowMode)uiInfo.UIFormShowMode;

            if ((LoadType)uiInfo.LandType == LoadType.Resources)
            {
                GameObject prefab = ResoucesMgr.Instance.Load<GameObject>(uiInfo.ResourcesPath, false);
                LoadUIFormFinish(uiFormName, prefab, parent, uiParam);
            }
            else if ((LoadType)uiInfo.LandType == LoadType.AssetBundle)
            {
                AbParam abParam = new AbParam();
                abParam.SceneName = "UI";
                abParam.AbName = uiInfo.AssetBundlePath;
                abParam.AssetName = uiInfo.AssetName;
                AssetManager.Instance.LoadAsset(abParam, (error, asset) =>
                {
                    if (string.IsNullOrEmpty(error)) LoadUIFormFinish(uiFormName, asset as GameObject, parent, uiParam);
                    else LoadUIFormFinish(uiFormName, null, parent, uiParam);
                });
            }
        }

        private void LoadUIFormFinish(string uiFormName, GameObject prefab, Transform parent, UIParam uiParam)
        {
            if (prefab == null)
            {
                ChangeUIFormState(uiFormName, UIFormState.Error);
                if (dicAllUIForms.ContainsKey(uiFormName)) dicAllUIForms.Remove(uiFormName);
                Debug.LogWarning(GetType() + "/LoadUIForm()/ load ui error! uiFormName:" + uiFormName);
                return;
            }

            if (dicUIFormState[uiFormName] == UIFormState.Loading) ChangeUIFormState(uiFormName, UIFormState.Open);

            GameObject item = Instantiate(prefab, parent);
            item.name = uiFormName;

            UIFormItem uIFormItem = item.GetComponent<UIFormItem>();
            if (uIFormItem == null) uIFormItem = item.AddComponent<UIFormItem>();
            uIFormItem.CurrentUIParam = uiParam;
            dicAllUIForms[uiFormName] = uIFormItem;

            Type type = Type.GetType(uiFormName);
            BaseUIForm baseUIForm = item.GetComponent<BaseUIForm>();
            if (type != null && baseUIForm == null)
            {
                item.AddComponent(type);
            }

            item.SetActive(dicUIFormState[uiFormName] == UIFormState.Open);

            if (!dicOpenUIForms.ContainsKey(uiFormName) && (dicUIFormState[uiFormName] == UIFormState.Open
                || dicUIFormState[uiFormName] == UIFormState.Disable)) dicOpenUIForms.Add(uiFormName, uIFormItem);
        }

        private void ChangeUIFormState(string uiFormName, UIFormState state)
        {
            UIConfigData uiInfo = uIConfig.GetDataByKey(uiFormName);

            UIFormShowMode uIFormShowMode = (UIFormShowMode)uiInfo.UIFormShowMode;
            if (uIFormShowMode == UIFormShowMode.HideOther)
            {
                if (state == UIFormState.Open) HideOpenUIForms();
                if (state == UIFormState.Close) DisplayOpenUIForms();
            }
            else if (uIFormShowMode == UIFormShowMode.ReverseChange)
            {
                if (state == UIFormState.Open) PushUIFormToStack(uiFormName);
                if (state == UIFormState.Close) ExitUIFormToStack();
            }

            dicUIFormState[uiFormName] = state;
        }

        private void HideOpenUIForms()
        {
            foreach (string uiFormName in dicOpenUIForms.Keys)
            {
                UIFormItem uiFormItem = dicOpenUIForms[uiFormName];
                if (uiFormItem != null) uiFormItem.gameObject.SetActive(false);
                ChangeUIFormState(uiFormName, UIFormState.Open);
            }
        }

        private void DisplayOpenUIForms()
        {
            foreach (string uiFormName in dicOpenUIForms.Keys)
            {
                UIFormItem uiFormItem = dicOpenUIForms[uiFormName];
                if (uiFormItem != null && dicUIFormState[uiFormName] == UIFormState.Open) uiFormItem.gameObject.SetActive(true);
                ChangeUIFormState(uiFormName, UIFormState.Hiding);
            }
        }

        private void PushUIFormToStack(string uiFormName)
        {
            if (staCurrentUIForms.Count > 0)
            {
                string topUIForm = staCurrentUIForms.Peek();
    
                if (dicOpenUIForms.ContainsKey(topUIForm))
                {
                    if (dicOpenUIForms[topUIForm] != null) dicOpenUIForms[topUIForm].gameObject.SetActive(false);
                }

                ChangeUIFormState(topUIForm, UIFormState.Disable);
            }

            staCurrentUIForms.Push(uiFormName);
        }

        private void ExitUIFormToStack()
        {
            if (staCurrentUIForms.Count >= 2)
            {
                string topUIForm = staCurrentUIForms.Pop();

                string nextUIForms = staCurrentUIForms.Peek();
                if (dicOpenUIForms.ContainsKey(nextUIForms))
                {
                    if (dicOpenUIForms[nextUIForms] != null) dicOpenUIForms[nextUIForms].gameObject.SetActive(true);
                    dicUIFormState[nextUIForms] =  UIFormState.Open;
                }
            }

            else if (staCurrentUIForms.Count == 1)
            {
                string topUIForm = staCurrentUIForms.Pop();
            }
        }

        private void ClearStack()
        {
            if (staCurrentUIForms.Count == 0) return;

            foreach (string uiFormName in staCurrentUIForms)
            {
                CloseUIForms(uiFormName);
            }
            staCurrentUIForms.Clear();
        }

        #endregion
    }
}