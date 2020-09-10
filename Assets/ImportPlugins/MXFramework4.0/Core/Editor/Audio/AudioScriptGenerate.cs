using UnityEngine;
using UnityEditor;
using Mx.Config;
using System.Text.RegularExpressions;
using System.IO;

namespace Mx.Audio
{
    public class AudioScriptGenerate 
    {
        [MenuItem("MXFramework/Audio/Generate Audio Param", false, 13)]
        public static void GenerateScript()
        {
            CreateAudioNames();
            AssetDatabase.Refresh();
        }

        private static void CreateAudioNames()
        {
            string template = GetTemplate(AudioDefine.Template_AUDIO_NAMES);

            string nameList = null;
            string typeList = null;

            AudioConfigDatabase audioConfigInfo = new AudioConfigDatabase();
            audioConfigInfo.Load();

            foreach (AudioConfigData info in audioConfigInfo.GetAllData())
            {
                nameList += SpliceFormName(info.Name, info.Des) + "\n";
                typeList += SpliceFormType(info.Name, info.Des) + "\n";
            }

            template = template.Replace("$AudioAttributes", nameList);
            template = template.Replace("$AudioType", typeList);

            GenerateScript("AudioNames", template);
        }

        private static string GetTemplate(string path)
        {
            TextAsset txt = Resources.Load<TextAsset>(path);
            return txt.text;
        }

        private static string SpliceFormName(string name, string des)
        {
            string note = string.Format(" /// <summary>{0}</summary> \n", des);

            string temp = name.Replace("Audio", null);
            string tempName = (Regex.Replace(temp, "(\\B[A-Z])", "_$1") + "_" + "AUDIO").ToUpper();

            string res = string.Format("public const string  {0} = \"" + name + "\"" + ";", tempName);

            return note + res;
        }

        private static string SpliceFormType(string name, string des)
        {
            string note = string.Format(" /// <summary>{0}</summary> \n", des);
            string res = name + ",";

            return note + res;
        }

        private static void GenerateScript(string dataName, string data)
        {
            dataName = AudioDefine.AudioScriptsPath + dataName + ".cs";
            if (File.Exists(dataName)) File.Delete(dataName);

            StreamWriter sr = File.CreateText(dataName);
            sr.WriteLine(data);
            sr.Close();
        }
    }
}