using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    // Start is called before the first frame update
    private List<GameObject> enemyList = new List<GameObject>();
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enemyList.IndexOf(collision.gameObject) < 0)
        {
            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("bulletEnemy"))
            {
                enemyList.Add(collision.gameObject);
            }
        }
    }
    public void StartScan()
    {
        enemyList.Clear();
    }
    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
