using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRuntimeData : MonoBehaviour
{
    public static GameRuntimeData instance;
    public List<BuildingsData> buildingsData;
    public BattleInfo batleInfo;
    private void Awake()
    {
        if(GameObject.FindGameObjectsWithTag("GameData").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
