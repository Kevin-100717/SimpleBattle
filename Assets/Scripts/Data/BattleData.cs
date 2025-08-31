using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : MonoBehaviour
{
    // Start is called before the first frame update
    public static BattleData instance;
    public List<BuffTemplate> buffs = new List<BuffTemplate>();
    void Start()
    {
        if(GameObject.FindObjectsOfType<BattleData>().Length>1)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
