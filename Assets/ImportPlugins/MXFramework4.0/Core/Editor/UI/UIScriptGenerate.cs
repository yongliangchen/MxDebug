using UnityEngine;
using UnityEditor;
using Mx.Config;
using System.IO;
using System.Text.RegularExpressions;

namespace Mx.UI
{
    public class UIScriptGenerate : MonoBehaviour
    {
        [MenuItem("MXFramework/UI/Generate UI Param", false, 11)]
        public static void GenerateUIParam()
        {
            CreateUIFormNames();
            AssetDatabase.Refresh();
        }

        [MenuItem("MXFramework/UI/Generate UI CSharp Script", false, 12)]
        public static void GenerateUICSharpScript()
        {
            CreateUICSharpScript();
            AssetDatabase.Refresh();
        }

        private static void CreateUIFormNames()
        {
            string template = GetTemplate(UIDefine.Template_UIFORM_NAMES);

            string uiFormNameLiset = null;
            string uiuiFormNameType = null;

            UIConfigDatabase uIConfigInfo = new UIConfigDatabase();
            uIConfigInfo.Load();

            foreach (UIConfigData info in uIConfigInfo.GetAllData())
            {
                uiFormNameLiset += SpliceFormName(info.Name, info.Des) + "\n";
                uiuiFormNameType += SpliceFormType(info.Name, info.Des) + "\n";
            }

            template = template.Replace("$UIAttributes", uiFormNameLiset);
            template = template.Replace("$UIType", uiuiFormNameType);

            GenerateScript("UIFormNames", template);
        }

        private static string SpliceFormName(string uiFormName, string des)
        {
            string note = string.Format(" /// <summary>{0}</summary> \n", des);

            string temp = uiFormName.Replace("UIForm", null);
            string tempName = (Regex.Replace(temp, "(\\B[A-Z])", "_$1") + "_" + "UIFORM").ToUpper();

            string res = string.Format("public const string  {0} = \"" + uiFormName + "\"" + ";", tempName);

            return note + res;
        }

        private static string SpliceFormType(string uiFormName, string des)
        {
            string note = string.Format(" /// <summary>{0}</summary> \n", des);
            string res = uiFormName + ",";

            return note + res;
        }

        private static void CreateUICSharpScript()
        {

            UIConfigDatabase uIConfigInfo = new UIConfigDatabase();
            uIConfigInfo.Load();

            foreach (UIConfigData info in uIConfigInfo.GetAllData())
            {
                string dataName = UIDefine.UIFormCSharpScriptsPath + info.Name + ".cs";
                if (!File.Exists(dataName))
                {
                    string template = GetTemplate(UIDefine.Template_UIFORM_CSHARP_BASE);
                    template = template.Replace("$classNote", info.Des);
                    template = template.Replace("$className", info.Name);
                    template = template.Replace("$messageType", info.Name + "Msg");

                    GenerateScript(info.Name, template);
                }
            }
        }

        private static string GetTemplate(string path)
        {
            //TextAsset txt = (TextAsset)AssetDatabase.LoadAssetAtPath(path, typeof(TextAsset));
            TextAsset txt = Resources.Load<TextAsset>(path);
            return txt.text;
        }

        private static void GenerateScript(string dataName, string data)
        {
            dataName = UIDefine.UIFormCSharpScriptsPath + dataName + ".cs";
            if (File.Exists(dataName)) File.Delete(dataName);

            StreamWriter sr = File.CreateText(dataName);
            sr.WriteLine(data);
            sr.Close();
        }
    }
}