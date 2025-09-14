using DG.Tweening;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI.Table;
public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public static LevelLoader Instance;
    public string dataFile = "";
    private JObject levelData;
    private JArray storeData;
    public Text title;
    public Transform frame;
    public GameObject battlePrefab;
    public GameObject bossPrefab;
    public GameObject emptyPrefab;
    public GameObject emptyHidePrefab;
    public GameObject guidePrefab;
    public GameObject connectPrefab;
    public Image black;
    public int currentLevel = 0;
    void Start()
    {
        Instance = this;
        dataFile = "level" + currentLevel.ToString() + ".json";
        Debug.Log("Load -> " + dataFile);
        levelData = ReadData(Application.streamingAssetsPath+"/"+dataFile);
        Debug.Log("Load -> store.json");
        storeData = ReadStore(Application.streamingAssetsPath + "/store.json");
        black.gameObject.SetActive(false);
        black.DOFade(0, 0);
        Load();
    }
    bool canShow(int i,int j)
    {
        bool flag = false;
        if ((int)storeData[currentLevel][i][j] == 2)
        {
            flag = true;
        }
        if ((int)storeData[currentLevel][i][j] == 1)
        {
            flag = true;
        }
        if (i < storeData[currentLevel].Count() - 1 && storeData[currentLevel][i + 1][j].ToObject<int>() == 2) flag = true;
        else if (j > 0 && (int)storeData[currentLevel][i][j - 1] == 2) flag = true;
        else if (j < storeData[currentLevel][i].Count() - 1 && (int)storeData[currentLevel][i][j + 1] == 2) flag = true;
        return flag;
    }
    void Load()
    {
        title.text = levelData["name"].ToString();
        int i = 0;
        foreach (JArray row in levelData["map"]) {
            int j = 0;
            foreach (int ind in row) {
                //1 初始节点 //2 完成 //初始节点只会显示，完成节点会显示旁边八个方向的可显示节点
                if(canShow(i,j) == false)
                {
                    Instantiate(emptyHidePrefab, frame);
                    j++;
                    continue;
                }
                JObject nodeData = JObject.Parse(levelData["mapData"][ind].ToString());
                GameObject node = null;
                switch (nodeData["type"].ToString())
                {
                    case "battle":
                        {
                            node = Instantiate(battlePrefab, frame);
                            node.GetComponent<NodeUI>().title = nodeData["title"].ToString();
                            node.GetComponent<NodeUI>().difficult = (int)nodeData["difficult"];
                            node.GetComponent<NodeUI>().content = nodeData["desc"].ToString();
                            node.GetComponent<NodeUI>().sceneId = nodeData["sceneId"].ToString();
                            break;
                        }
                    case "boss":
                        {
                            node = Instantiate(bossPrefab, frame);
                            node.GetComponent<NodeUI>().title = nodeData["title"].ToString();
                            node.GetComponent<NodeUI>().difficult = (int)nodeData["difficult"];
                            node.GetComponent<NodeUI>().content = nodeData["desc"].ToString();
                            node.GetComponent<NodeUI>().sceneId = nodeData["sceneId"].ToString();
                            break;
                        }
                    case "blank":
                        {
                            //node = Instantiate(emptyPrefab, frame);
                            //四周至少有一个非id为0的节点则显示为空节点
                            bool show = false;
                            if(i > 0 && (int)row[j] != 0) show = true;
                            else if(i < levelData["map"].Count() - 1 && levelData["map"][i+1][j].ToObject<int>() != 0) show = true;
                            else if(j > 0 && (int)row[j-1] != 0) show = true;
                            else if(j < row.Count() - 1 && (int)row[j+1] != 0) show = true;
                            if(show) node = Instantiate(emptyPrefab, frame);
                            else node = Instantiate(emptyHidePrefab, frame);
                            break;
                        }
                    case "message":
                        {
                            node = Instantiate(guidePrefab, frame);
                            node.GetComponent<NodeUI>().title = nodeData["title"].ToString();
                            node.GetComponent<NodeUI>().content = nodeData["content"].ToString();
                            break;
                        }
                }
                j++;
                // [ 1, 1, 0, 0 ] //up right down left
                //生成连接节点
                if(nodeData["connect"] == null) continue;
                int x = 0;
                foreach(int connect in nodeData["connect"])
                {
                    if(connect == 1)
                    {
                        GameObject c = Instantiate(connectPrefab, node.transform);
                        c.transform.eulerAngles = new Vector3(0,0, -90 * x);
                    }
                    x++;
                }
            }
            i++;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public JObject ReadData(string path)
    {
        string readData;
        string fileUrl = path;
        using (StreamReader sr = File.OpenText(fileUrl))
        {
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return JObject.Parse(readData);
    }
    public JArray ReadStore(string path)
    {
        string readData;
        string fileUrl = path;
        using (StreamReader sr = File.OpenText(fileUrl))
        {
            readData = sr.ReadToEnd();
            sr.Close();
        }
        return JArray.Parse(readData);
    }
    public void LoadLevel(string sceneId)
    {
        black.gameObject.SetActive(true);
        black.DOFade(1, 0.8f).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneId);
        });
    }
}
