using System.IO;
using UnityEngine;

namespace Mx.Audio
{
    public sealed class AudioDefine 
    {
        private static string audioScriptsPath = Application.dataPath + "/Scripts/AutoGenerate/";
        /// <summary>声音模块自动生成脚本存放路径</summary>
        public static string AudioScriptsPath
        {
            get
            {
                if (!Directory.Exists(audioScriptsPath)) Directory.CreateDirectory(audioScriptsPath);
                return audioScriptsPath;
            }
        }
        public const string Template_AUDIO_NAMES = "Template/Audio/Template_AudioNames";
    }

    /// <summary>UI窗体状态</summary>
    public enum AudioState
    {
        /// <summary>未知状态</summary>
        None,
        /// <summary>加载当中</summary>
        Loading,
        /// <summary>播放状态</summary>
        Play,
        /// <summary>暂停状态</summary>
        Pause,
        /// <summary>停止状态</summary>
        Stop,
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

    /// <summary>
    /// 声音回调
    /// </summary>
    /// <param name="audioName">声音名字</param>
    /// <param name="state">状态</param>
    /// <param name="time">声音的长度</param>
    /// <param name="playTime">播放时间</param>
    public delegate void DelAudioCallback(string audioName, AudioState state, float time, float playTime);
}