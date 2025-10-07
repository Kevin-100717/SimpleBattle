using DG.Tweening;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public static LevelLoader Instance;
    public string dataFile = "";
    private JObject levelData;
    private List<LevelInfo> levels = new List<LevelInfo>();
    public Text title;
    public Transform frame;
    public GameObject battlePrefab;
    public GameObject bossPrefab;
    public GameObject choosePrefab;
    public Image black;
    public int currentLevel = 0;
    public List<NodeData> setNodes = new List<NodeData>();
    private void Start()
    {
        Instance = this;
        black.DOFade(0, 0);
        black.gameObject.SetActive(false);
        levelData = ReadData(Application.streamingAssetsPath+"/"+dataFile);
        title.text = levelData["name"].ToString();
        ReadLevelData();
        StoreData store;
        if(File.Exists(Application.streamingAssetsPath + "/store.json")){
            store = JsonConvert.DeserializeObject<StoreData>(ReadData(Application.streamingAssetsPath + "/store.json").ToString());
            Debug.Log("Store.json loaded");
        }else {
            Debug.Log("new store file");
            store = generateLevel();
        }
        ShowMapData(store);
    }
    private void ShowMapData(StoreData sd)
    {
        float height = 300;
        foreach(List<List<int>> row in sd.levels)
        {
            float x = 400;
            foreach(List<int> node in row)
            {
                int id = node[0];
                LevelInfo li = levels[id];
                Debug.Log(li.title);
                GameObject btn;
                NodeUI nui;
                Debug.Log(height + UnityEngine.Random.Range(-60, 60));
                switch (li.type)
                {
                    case LevelInfo.LevelType.Battle:
                        btn = Instantiate(battlePrefab, frame);
                        btn.transform.localPosition = new Vector3(x, height + UnityEngine.Random.Range(-60, 60), 0);
                        nui = btn.GetComponent<NodeUI>();
                        nui.title = li.title;
                        nui.content = li.content;
                        nui.sceneId = li.sceneId;
                        nui.difficult = li.difficult;
                        break;
                    case LevelInfo.LevelType.Choose:
                        btn = Instantiate(choosePrefab, frame);
                        btn.transform.localPosition = new Vector3(x, height + UnityEngine.Random.Range(-60, 60), 0);
                        nui = btn.GetComponent<NodeUI>();
                        nui.title = li.title;
                        nui.content = li.content;
                        nui.sceneId = li.sceneId;
                        nui.difficult = li.difficult;
                        break;
                }
                x += 400 + UnityEngine.Random.Range(-60, 60);
            }
            height += 400 + UnityEngine.Random.Range(-60, 60);
        }
        frame.GetComponent<RectTransform>().sizeDelta = new Vector2(frame.GetComponent<RectTransform>().sizeDelta.x, height);
        frame.parent.parent.gameObject.GetComponent<ScrollRect>().normalizedPosition = new Vector2(0, 0);
    }
    private void ReadLevelData()
    {
        foreach(JObject level in JArray.Parse(levelData["mapData"].ToString()))
        {
            string desc = level["desc"].ToString();
            string title = level["title"].ToString();
            int difficult = int.Parse(level["difficult"].ToString());
            string type = level["type"].ToString();
            string sceneId = level["sceneId"].ToString();
            LevelInfo levelInfo = new LevelInfo()
            {
                type = (LevelInfo.LevelType)(Enum.Parse(typeof(LevelInfo.LevelType), type)),
                title = title,
                content = desc,
                difficult = difficult,
                sceneId = sceneId
            };
            levels.Add(levelInfo);
        }
    }
    private StoreData generateLevel()
    {
        // 读取关卡类型和下标
        JArray mapData = (JArray)levelData["mapData"];
        Dictionary<LevelInfo.LevelType, List<int>> typeToIndexes = new Dictionary<LevelInfo.LevelType, List<int>>();
        for (int i = 0; i < mapData.Count; i++)
        {
            string typeStr = mapData[i]["type"].ToString();
            LevelInfo.LevelType typeEnum = (LevelInfo.LevelType)Enum.Parse(typeof(LevelInfo.LevelType), typeStr);
            if (!typeToIndexes.ContainsKey(typeEnum)) typeToIndexes[typeEnum] = new List<int>();
            typeToIndexes[typeEnum].Add(i);
        }
        Dictionary<NodeData.NodeType, LevelInfo.LevelType> nodeTypeMap = new Dictionary<NodeData.NodeType, LevelInfo.LevelType>
        {
            { NodeData.NodeType.RandomBattle, LevelInfo.LevelType.Battle },
            { NodeData.NodeType.CustomBattle, LevelInfo.LevelType.Battle },
            { NodeData.NodeType.RamdonChoose, LevelInfo.LevelType.Choose },
            { NodeData.NodeType.CostomChoose, LevelInfo.LevelType.Choose }
        };
        List<List<List<int>>> levels = new List<List<List<int>>>();
        System.Random rand = new System.Random();
        List<List<int>> prevRowConns = null;

        for (int row = 0; row < setNodes.Count; row++)
        {
            NodeData nodeData = setNodes[row];
            List<List<int>> rowNodes = new List<List<int>>();
            int minN = Math.Min(nodeData.min_n, nodeData.max_n);
            int maxN = Math.Max(nodeData.min_n, nodeData.max_n);
            int nodeCount = nodeData.randomNum ? rand.Next(minN, maxN + 1) : nodeData.nodes.Count;

            for (int n = 0; n < nodeCount; n++)
            {
                int levelIdx = -1;
                if (nodeData.randomNum)
                {
                    List<int> candidates = new List<int>();
                    if (typeToIndexes.ContainsKey(LevelInfo.LevelType.Battle))
                        candidates.AddRange(typeToIndexes[LevelInfo.LevelType.Battle]);
                    if (typeToIndexes.ContainsKey(LevelInfo.LevelType.Choose))
                        candidates.AddRange(typeToIndexes[LevelInfo.LevelType.Choose]);
                    levelIdx = candidates.Count > 0 ? candidates[rand.Next(candidates.Count)] : 0;
                }
                else
                {
                    var rowNode = nodeData.nodes[n];
                    if (rowNode.type == NodeData.NodeType.CustomBattle || rowNode.type == NodeData.NodeType.CostomChoose)
                    {
                        levelIdx = rowNode.id;
                    }
                    else
                    {
                        LevelInfo.LevelType mappedType = nodeTypeMap[rowNode.type];
                        List<int> candidates = typeToIndexes.ContainsKey(mappedType) ? typeToIndexes[mappedType] : new List<int>();
                        levelIdx = candidates.Count > 0 ? candidates[rand.Next(candidates.Count)] : 0;
                    }
                }
                List<int> conn = new List<int>();
                if (row < setNodes.Count - 1)
                {
                    var nextNodeData = setNodes[row + 1];
                    int nextMinN = Math.Min(nextNodeData.min_n, nextNodeData.max_n);
                    int nextMaxN = Math.Max(nextNodeData.min_n, nextNodeData.max_n);
                    int nextCount = nextNodeData.randomNum ? rand.Next(nextMinN, nextMaxN + 1) : nextNodeData.nodes.Count;

                    // 1. 必须连一个
                    if (nextCount > 0)
                    {
                        int mustConn = rand.Next(nextCount);
                        conn.Add(mustConn);

                        // 2. 随机多连其它节点
                        int extraConnNum = rand.Next(0, nextCount); // 可能为0
                        HashSet<int> connSet = new HashSet<int>(conn);
                        while (connSet.Count < Math.Min(extraConnNum + 1, nextCount))
                        {
                            connSet.Add(rand.Next(nextCount));
                        }
                        conn = new List<int>(connSet);
                    }
                }
                List<int> nodeInfo = new List<int> { levelIdx };
                nodeInfo.AddRange(conn);
                rowNodes.Add(nodeInfo);
            }
            levels.Add(rowNodes);

            // 3. 保证下一层每个节点都被上层连接（第一层除外）
            if (row > 0 && levels.Count > 1)
            {
                int curRowIdx = levels.Count - 1;
                int prevRowIdx = curRowIdx - 1;
                int curNodeCount = levels[curRowIdx].Count;
                bool[] hasConn = new bool[curNodeCount];
                foreach (var prevNode in levels[prevRowIdx])
                {
                    for (int i = 1; i < prevNode.Count; i++)
                    {
                        int connIdx = prevNode[i];
                        if (connIdx >= 0 && connIdx < curNodeCount)
                            hasConn[connIdx] = true;
                    }
                }
                for (int i = 0; i < curNodeCount; i++)
                {
                    if (!hasConn[i])
                    {
                        // 随机让上一层某节点连它
                        int prevNodeIdx = rand.Next(levels[prevRowIdx].Count);
                        levels[prevRowIdx][prevNodeIdx].Add(i);
                    }
                }
            }
        }

        StoreData store = new StoreData();
        store.currentIndex = 0;
        store.levels = levels;
        // 保存为json
        string json = JsonConvert.SerializeObject(store, Formatting.Indented);
        File.WriteAllText(Path.Combine(Application.streamingAssetsPath, "store.json"), json);
        Debug.Log("Map generated: store.json");
        return store;
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
    public void LoadLevel(string sceneId)
    {
        black.gameObject.SetActive(true);
        black.DOFade(1, 0.8f).OnComplete(() =>
        {
            SceneManager.LoadScene(sceneId);
        });
    }
}
