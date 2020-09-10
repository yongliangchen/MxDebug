using System;
using UnityEngine;

namespace Mx.Audio
{
    public sealed class AudioItem : MonoBehaviour
    {
        #region 数据申明

        private Transform audioRoot;
        public AudioState State { get; set; }
        public string AudioName { get; set; }
        public AudioSource Audio { get; set; }
        public AudioClip Clip { get;set; }
        /// <summary>声音的长度</summary>
        public float Time { get; private set; }
        /// <summary>当前播放的时间</summary>
        public float PlayTime { get; private set; }
        /// <summary>是否正在播放</summary>
        private bool isPlay = false;
        public Transform target { get; set; }

        /// <summary>声音回调</summary>
        public DelAudioCallback OnAudioCallback { get; set; }

        #endregion

        #region Unity函数

        private void OnEnable()
        {
            if (isPlay) FurtherAudioPlayback();
        }

        private void Awake()
        {
            if (audioRoot == null) audioRoot = new GameObject("AudioRoot").transform;
        }

        private void Update()
        {
            if (isPlay)
            {
                PlayTime = Audio.time;
                if (Time <= PlayTime)
                {
                    PlayTime = Time;
                    isPlay = false;

                    State = AudioState.Stop;

                    if (OnAudioCallback != null) OnAudioCallback(AudioName, State, Time, PlayTime);
                }

                if (OnAudioCallback != null) OnAudioCallback(AudioName, State, Time, PlayTime);
            }
        }

        #endregion

        #region 公开函数

        /// <summary>播放声音</summary>
        public void Play(bool mute, float volume)
        {
            if (OnAudioCallback != null) OnAudioCallback(AudioName, State, Time, PlayTime);

            Audio.clip = Clip;
            Time = Clip.length;

            if (target == null) { transform.SetParent(audioRoot); }
            else
            {
                transform.SetParent(target);
                Audio.spatialBlend = 1;
            }

            Audio.mute = mute;
            Audio.volume = volume;
            Audio.Play();
            isPlay = true;
        }

        public float GetPlayProgress()
        {
            if (Clip == null) return 0;
            return PlayTime / Time;
        }

        /// <summary>重新播放</summary>
        public void Replay()
        {
            isPlay = true;
            Audio.Play();
            State = AudioState.Play;
            if (OnAudioCallback != null) OnAudioCallback(AudioName, State, Time, PlayTime);
            ChangeProgress(0);
        }

        /// <summary>继续播放</summary>
        public void FurtherAudioPlayback()
        {
            isPlay = true;
            Audio.Play();
            State = AudioState.Play;
            if (OnAudioCallback != null) OnAudioCallback(AudioName, State, Time, PlayTime);
            ChangeProgress(PlayTime / Time);
        }

        /// <summary>暂停播放</summary>
        public void Pause()
        {
            Audio.Pause();
            isPlay = false;

            if (OnAudioCallback != null) OnAudioCallback(AudioName, State, Time, PlayTime);
        }

        /// <summary>停止播放</summary>
        public void Stop()
        {
            Audio.Stop();
            isPlay = false;

            if (OnAudioCallback != null) OnAudioCallback(AudioName, State, Time, PlayTime);
        }

        /// <summary>
        /// 切换进度
        /// </summary>
        /// <param name="progress">0-1</param>
        public void ChangeProgress(float progress)
        {
            if (progress >= 1)
            {
                if (Audio.loop)
                {
                    isPlay = true;
                    Audio.time = 0;
                    Audio.Play();
                }
            }
            else
            {
                PlayTime = Time * Mathf.Clamp01(progress);
                Audio.time = PlayTime;
                isPlay = true;
                Audio.Play();
            }
        }

        #endregion
    }
}