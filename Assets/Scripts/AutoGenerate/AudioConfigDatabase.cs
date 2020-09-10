using UnityEngine;
using System.Collections.Generic;
using Mx.Util;
using System;

namespace Mx.Config
{
    [Serializable]
	public class AudioConfigData
	{
		public string Name;
		public int LandType;
		public string ResourcesPath;
		public string SceneName;
		public string AssetBundlePath;
		public string AssetName;
		public string Des;
	}

	public class AudioConfigDatabase:IDatabase
	{
		public const uint TYPE_ID = 1;
		public const string DATA_PATH = "AudioConfig";
       
		private string[][] m_datas;
        private Dictionary<string, AudioConfigData> dicData = new Dictionary<string, AudioConfigData>();
        private List<AudioConfigData> listData = new List<AudioConfigData>();

		public AudioConfigDatabase(){}

		public uint TypeID()
		{
			return TYPE_ID;
		}

		public string DataPath()
		{
			return ConfigDefine.GetResoucesConfigOutPath + DATA_PATH;
		}

        public void Load()
        {
          dicData.Clear();
          listData.Clear();

           TextAsset textAsset = Resources.Load<TextAsset>(DataPath());
           string str = textAsset.text;
           if (string.IsNullOrEmpty(str))
           {
               Debug.LogError(GetType() + "/Load()/ load config error! path:" + DataPath());
           }
         
          string textData = StringEncrypt.DecryptDES(str);
          m_datas = CSVConverter.SerializeCSVData(textData);
          Serialization();

        }

		private void Serialization()
		{
			for(int cnt = 0; cnt < m_datas.Length; cnt++)
			{
                AudioConfigData m_tempData = new AudioConfigData();
			    m_tempData.Name = m_datas[cnt][0];
		
			if(!int.TryParse(m_datas[cnt][1], out m_tempData.LandType))
			{
				m_tempData.LandType = 0;
			}

		m_tempData.ResourcesPath = m_datas[cnt][2];
		m_tempData.SceneName = m_datas[cnt][3];
		m_tempData.AssetBundlePath = m_datas[cnt][4];
		m_tempData.AssetName = m_datas[cnt][5];
		m_tempData.Des = m_datas[cnt][6];
                if(!dicData.ContainsKey(m_datas[cnt][0]))
                {
                    dicData.Add(m_datas[cnt][0], m_tempData);
                    listData.Add(m_tempData);
                }
			}
		}

        public AudioConfigData GetDataByKey(string key)
        {
            AudioConfigData data;
            dicData.TryGetValue(key, out data);
            return data;
        }

		public int GetCount()
		{
			return listData.Count;
		}

        public List <AudioConfigData> GetAllData()
        {
            return listData;
        }

	}
}
