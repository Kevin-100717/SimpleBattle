using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreate : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int loop;
    public float interval;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init(GameObject p,int l,float i) { 
        enemyPrefab = p;
        loop = l;
        interval = i;
        StartCoroutine(CreateEnemy());
    }
    IEnumerator CreateEnemy() {
        for(int i = 0; i < loop; i++) {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(interval);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
