using UnityEngine;
using System.IO;

namespace kai
{

    /// <summary>
    /// セーブデータ
    /// </summary>
    [System.Serializable]
    public class SaveData
    {
        // セーブするデータ
        public int LEVEL;
        public int COIN;

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// データ読み込み
        /// </summary>
        public void LoadFile()
        {
            string datastr = "";
            StreamReader reader;
            reader = new StreamReader(Application.dataPath + "/@ColorClash/Resources/SaveData.json");
            datastr = reader.ReadToEnd();
            reader.Close();

            SaveData data = JsonUtility.FromJson<SaveData>(datastr);
            LEVEL = data.LEVEL;
            COIN = data.COIN;

            Debug.Log("【SAVEDATA/LOADED】"+"LEVEL:" + LEVEL + "/COIN:" + COIN);
        }

        //-----------------------------------------------------------------------------------------
        /// <summary>
        /// データ書き込み
        /// </summary>
        public void SaveFile()
        {
            StreamWriter writer;
            writer = new StreamWriter(Application.dataPath + "/@ColorClash/Resources/SaveData.json", false);

            string jsonstr = JsonUtility.ToJson(this);
            writer.Write(jsonstr);
            writer.Flush();
            writer.Close();

            Debug.Log("【SAVEDATA/SAVED】" + "LEVEL:" + LEVEL + " COIN:" + COIN);
        }
    }

} // namespace
