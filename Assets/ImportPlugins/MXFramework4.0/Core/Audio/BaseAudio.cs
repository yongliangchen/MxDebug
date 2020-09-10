using System.Collections.Generic;
using Mx.Config;
using Mx.Res;
using UnityEngine;
using System.Linq;

namespace Mx.Audio
{
    /// <summary>声音模块的的父类</summary>
    public abstract class BaseAudio : MonoBehaviour
    {
        #region 数据申明

        private AudioConfigDatabase audioConfig;
        private Dictionary<string, AudioItem> dicAllAudios = new Dictionary<string, AudioItem>();

        private bool mute = false;
        public bool Mute
        {
            get { return mute; }
            set
            {
                mute = value;
                MuteToggle();
            }
        }

        private float volume=1;
        public float Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                SetVolume();
            }
        }

        #endregion

        #region 公开函数

        /// <summary>初始化</summary>
        public void Init(AudioConfigDatabase audioConfig)
        {
            this.audioConfig = audioConfig;
        }

        /// <summary>播放一个2D声音</summary>
        public void PlayAudio2D(string audioName, bool isLoop = false, DelAudioCallback audioCallback = null)
        {
            PlayAudio(audioName, null, isLoop, audioCallback);
        }

        /// <summary>播放一个3D声音</summary>
        public void PlayAudio3D(string audioName, Transform target, bool isLoop = false, DelAudioCallback audioCallback = null)
        {
            PlayAudio(audioName, target, isLoop, audioCallback);
        }

        /// <summary>设置循环播放</summary>
        public void SetLoop(string audioName, bool isLoop)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            if (audioItem != null) audioItem.Audio.loop = isLoop;
        }

        /// <summary>关闭声音播放</summary>
        public void CloseAudios(params string[] audioNames)
        {
            for(int i=0;i< audioNames.Length;i++)
            {
                ChangeState(audioNames[i],AudioState.Stop);
            }
        }

        /// <summary>关闭所有声音播放</summary>
        public void CloseAllAudios()
        {
            string[] audioNames = dicAllAudios.Keys.ToArray<string>();
            CloseAudios(audioNames);
        }

        /// <summary>继续播放声音（从暂停状态进度继续播放）</summary>
        public void FurtherAudioPlayback(params string[] audioNames)
        {
            for (int i = 0; i < audioNames.Length; i++)
            {
                string audioName = audioNames[i];
                AudioItem audioItem= GetAudioItemByName(audioName);
                if (audioItem != null) audioItem.FurtherAudioPlayback();
            }
        }

        /// <summary>暂停声音播放</summary>
        public void PauseAudios(params string[] audioNames)
        {
            for (int i = 0; i < audioNames.Length; i++)
            {
                ChangeState(audioNames[i],AudioState.Pause);
            }
        }

        /// <summary>暂停所有声音播放</summary>
        public void PauseAllAudios()
        {
            string[] audioNames = dicAllAudios.Keys.ToArray<string>();
            PauseAudios(audioNames);
        }

        /// <summary>重新播放声音（进度从0开始）</summary>
        public void ReplayAudios(params string[] audioNames)
        {
            for (int i = 0; i < audioNames.Length; i++)
            {
                string audioName = audioNames[i];
                AudioItem audioItem= GetAudioItemByName(audioName);
                if (audioItem != null) audioItem.Replay();
            }
        }

        /// <summary>重新播放所有声音（进度从0开始）</summary>
        public void ReplayAllAudios()
        {
            string[] audioNames = dicAllAudios.Keys.ToArray<string>();
            ReplayAudios(audioNames);
        }

        /// <summary>移除声音及相关声音组件</summary>
        public void RemoveAudios(params string[] audioNames)
        {
            for(int i=0;i< audioNames.Length;i++)
            {
                string audioName = audioNames[i];
                AudioItem audioItem = GetAudioItemByName(audioName);

                if (dicAllAudios.ContainsKey(audioName)) dicAllAudios.Remove(audioName);
                if (audioItem != null) Destroy(audioItem.gameObject);
            }
        }

        /// <summary>移除所有声音及相关声音组件</summary>
        public void RemoveAllAudios()
        {
            string[] audioNames = dicAllAudios.Keys.ToArray<string>();
            RemoveAudios(audioNames);
        }

        /// <summary>获取AudioItem</summary>
        public AudioItem GetAudioItemByName(string audioName)
        {
            AudioItem audioItem;
            dicAllAudios.TryGetValue(audioName, out audioItem);
            return audioItem;
        }

        /// <summary>获取声音状态</summary>
        public AudioState GetAudioStateByName(string audioName)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            if (audioItem == null) return AudioState.Error;
            return audioItem.State;
        }

        /// <summary>获取声音播放进度（取值范围：0-1）</summary>
        public float GetProgressByName(string audioName)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            if (audioItem == null) return 0;
            return audioItem.GetPlayProgress();
        }

        /// <summary>获取声音播放时间长度（单位：秒）</summary>
        public float GetPlayTimeByName(string audioName)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            if (audioItem == null) return 0;
            return audioItem.PlayTime;
        }

        /// <summary>获取声音时间长度（单位：秒）</summary>
        public float GetTimeByName(string audioName)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            if (audioItem == null) return 0;
            return audioItem.Time;
        }

        /// <summary>设置播放进度（0-1）</summary>
        public void SetProgress(string audioName, float progress)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            if (audioItem != null) audioItem.ChangeProgress(progress);
        }

        #endregion

        #region 私有函数

        private void PlayAudio(string audioName, Transform target, bool isLoop, DelAudioCallback audioCallback)
        {
            AudioConfigData audioData = audioConfig.GetDataByKey(audioName);
            if (audioData == null)
            {
                Debug.LogError(GetType() + "/PlayAudio()/ play audio error! audioName:" + audioName);
                return;
            }

            if (dicAllAudios.ContainsKey(audioName))
            {
                AudioItem audioItem = GetAudioItemByName(audioName);
                if (audioItem != null && audioItem.State != AudioState.Loading && audioItem.State != AudioState.Error)
                {
                    audioItem.AudioName = audioName;
                    audioItem.OnAudioCallback = audioCallback;
                    audioItem.target = target;
                    ChangeState(audioName,AudioState.Play);
                }
            }
            else
            {
                GameObject item = new GameObject(audioName);
                AudioSource audioSource = item.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = isLoop;
                AudioItem audioItem = item.AddComponent<AudioItem>();
                audioItem.AudioName = audioName;
                audioItem.OnAudioCallback = audioCallback;
                audioItem.target = target;
                audioItem.Audio = audioSource;
                dicAllAudios.Add(audioName, audioItem);
                ChangeState(audioName, AudioState.Loading);
            }
        }

        private void MuteToggle()
        {
            foreach (string audioName in dicAllAudios.Keys)
            {
                AudioItem audioItem = dicAllAudios[audioName];
                if (audioItem != null) audioItem.Audio.mute = Mute;
            }
        }

        private void SetVolume()
        {
            foreach (string audioName in dicAllAudios.Keys)
            {
                AudioItem audioItem = dicAllAudios[audioName];
                if (audioItem != null) audioItem.Audio.volume = Volume;
            }
        }

        private void ChangeState(string audioName,AudioState state)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            AudioConfigData audioData = audioConfig.GetDataByKey(audioName);
            if (audioData == null || audioItem == null) return;

            audioItem.State = state;

            switch (state)
            {
                case AudioState.Loading: LoadingState(audioData); break;
                case AudioState.Play: PlayState(audioName, audioItem); break;
                case AudioState.Pause: PauseState(audioItem); break;
                case AudioState.Stop: StopState(audioName, audioItem); break;
                case AudioState.Error: ErrorState(audioName); break;
            }
        }

        private void LoadingState(AudioConfigData audioData)
        {
            Load(audioData);
        }

        private void PlayState(string audioName,AudioItem audioItem)
        {
            audioItem.Play(Mute, volume);
            if(!dicAllAudios.ContainsKey(audioName)) dicAllAudios.Add(audioName, audioItem);
        }

        private void PauseState(AudioItem audioItem)
        {
            audioItem.Pause();
        }

        private void StopState(string audioName, AudioItem audioItem)
        {
            audioItem.Stop();
            if (dicAllAudios.ContainsKey(audioName)) dicAllAudios.Remove(audioName);
        }

        private void ErrorState(string audioName)
        {
            RemoveAudios(audioName);
        }

        private void Load(AudioConfigData audioData)
        {
            LoadType loadType = (LoadType)audioData.LandType;

            if (loadType == LoadType.Resources)
            {
                AudioClip audioClip = ResoucesMgr.Instance.Load<AudioClip>(audioData.ResourcesPath, false);
                if (audioClip != null) LoadFinish(audioData.Name,audioClip);
                else
                {
                    Debug.LogError(GetType() + "/Load()/ load audio error! audioName:" + audioData.Name);
                    ChangeState(audioData.Name, AudioState.Error);
                }
            }
            else if (loadType == LoadType.AssetBundle)
            {
                AbParam abParam = new AbParam();
                abParam.SceneName = audioData.SceneName;
                abParam.AbName = audioData.AssetBundlePath;
                abParam.AssetName = audioData.AssetName;

                AssetManager.Instance.LoadAsset(abParam, (error, asset) =>
                {
                    if (string.IsNullOrEmpty(error))
                    {
                        LoadFinish(audioData.Name, asset as AudioClip);
                    }
                    else
                    {
                        Debug.LogError(GetType() + "/Load()/ load audio error! audioName:" + audioData.Name);
                        ChangeState(audioData.Name, AudioState.Error);
                    }
                });
            }
        }

        private void LoadFinish(string audioName, AudioClip audioClip)
        {
            AudioItem audioItem = GetAudioItemByName(audioName);
            if (audioItem == null) return;

            audioItem.Clip = audioClip;
            if (audioItem.State == AudioState.Loading) ChangeState(audioName, AudioState.Play);
        }

        #endregion
    }
}