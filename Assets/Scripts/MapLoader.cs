using Game.Data;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    public static MapLoader instance;
    public BattleData battleData;
    public string mapDataJsonFilePath;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        battleData = JsonConvert.DeserializeObject<BattleData>(ReadData(mapDataJsonFilePath));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string ReadData(string path)
    {
        string fileUrl = Application.streamingAssetsPath + path;
        using (StreamReader sr = new StreamReader(fileUrl))
        {
            string readData = sr.ReadToEnd();
            sr.Close();
            return readData;
        }
    }
}
