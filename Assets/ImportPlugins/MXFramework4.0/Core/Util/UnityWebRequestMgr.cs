/***
 * 
 *    Title: MXFramework
 *           主题: UnityWebRequest工具
 *    Description: 
 *           功能：1.UnityWebRequest常用的API进行封装
 * 
 *    Date: 2020
 *    Version: v4.0版本
 *    Modify Recoder: 
 *      
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Mx.Util
{
    public class UnityWebRequestMgr : MonoBehaviour
    {
        #region 数据申明

        private static UnityWebRequestMgr instance;
        public static UnityWebRequestMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject mounter = new GameObject("UnityWebRequestMgr");
                    instance = mounter.AddComponent<UnityWebRequestMgr>();
                }
                return instance;
            }
        }

        #endregion

        #region 公开函数

        public void Get(string url, Action<UnityWebRequest> actionResult)
        {
            StartCoroutine(GetAsyn(url, actionResult));
        }

        public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            StartCoroutine(DownloadFileAsyn(url, downloadFilePathAndName, actionResult));
        }

        public void GetTexture(string url, Action<Texture2D> actionResult)
        {
            StartCoroutine(GetTextureAsyn(url, actionResult));
        }

        public void GetText(string url, Action<string> actionResult)
        {
            StartCoroutine(GetTextAsyn(url, actionResult));
        }

        public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
        {
            StartCoroutine(GetAssetBundleAsyn(url, actionResult));
        }

        public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            StartCoroutine(GetAudioClipAsyn(url, actionResult, audioType));
        }

        public void Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
        {
            StartCoroutine(PostAsyn(serverURL, lstformData, actionResult));
        }

        public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
        {
            StartCoroutine(UploadByPutAsyn(url, contentBytes, actionResult, ""));
        }

        #endregion

        #region 私有函数

        private IEnumerator GetAsyn(string url, Action<UnityWebRequest> actionResult)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();
                if (actionResult != null) { actionResult(uwr); }
            }
        }

        private IEnumerator DownloadFileAsyn(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
            yield return uwr.SendWebRequest();
            if (actionResult != null) { actionResult(uwr); }
        }

        private IEnumerator GetTextureAsyn(string url, Action<Texture2D> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;
            yield return uwr.SendWebRequest();
            Texture2D t = null;
            if (!(uwr.isNetworkError || uwr.isHttpError)) { t = downloadTexture.texture; }
            if (t == null) Debug.LogError(GetType() + "GetTextureAsyn()/ Get Texture is error! url:" + url);
            if (actionResult != null) { actionResult(t); }
        }

        private IEnumerator GetTextAsyn(string url, Action<string> actionResult)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            string t = request.downloadHandler.text;
            if (string.IsNullOrEmpty(t)) Debug.LogError(GetType() + "GetTextAsyn()/ Get Text is error! url:" + url);
            if (actionResult != null) { actionResult(t); }
        }

        private IEnumerator GetAssetBundleAsyn(string url, Action<AssetBundle> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(uwr.url, uint.MaxValue);
            uwr.downloadHandler = handler;
            yield return uwr.SendWebRequest();
            AssetBundle bundle = null;
            if (!(uwr.isNetworkError || uwr.isHttpError)) { bundle = handler.assetBundle; }
            if (bundle==null) Debug.LogError(GetType() + "GetAssetBundleAsyn()/ Get AssetBundle is error! url:" + url);
            if (actionResult != null) { actionResult(bundle); }
        }

        private IEnumerator GetAudioClipAsyn(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                yield return uwr.SendWebRequest();
                if (!(uwr.isNetworkError || uwr.isHttpError))
                {
                    if (actionResult != null) { actionResult(DownloadHandlerAudioClip.GetContent(uwr)); }
                }
                else Debug.LogError(GetType() + "GetAudioClipAsyn()/ Get AudioClip is error! url:" + url);
            }
        }

        private IEnumerator PostAsyn(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
        {
            UnityWebRequest uwr = UnityWebRequest.Post(serverURL, lstformData);
            yield return uwr.SendWebRequest();
            if (actionResult != null) { actionResult(uwr); }
        }

        private IEnumerator UploadByPutAsyn(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
        {
            UnityWebRequest uwr = new UnityWebRequest();
            UploadHandler uploader = new UploadHandlerRaw(contentBytes);

            uploader.contentType = contentType;

            uwr.uploadHandler = uploader;

            yield return uwr.SendWebRequest();

            bool res = true;
            if (uwr.isNetworkError || uwr.isHttpError) { res = false; }
            if (actionResult != null) { actionResult(res); }
        }

        #endregion
    }
}