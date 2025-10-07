using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;
[System.Serializable]
public class StoreData
{
    public int currentIndex = 0;
    public List<List<List<int>>> levels = new List<List<List<int>>>(); //col row (id conn1 conn2 ...)
}
[System.Serializable]
public class LevelInfo
{
    public enum LevelType
    {
        Battle,
        Boss,
        Choose
    }
    public LevelType type;
    public string title;
    public string content;
    public string sceneId;
    public int chooseEventIndex;
    public int difficult;

}