using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;
    public List<Transform> spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
    public void StartSpawn(List<DatasEntity> d)
    {
        StartCoroutine(spawn(d));
    }
    IEnumerator spawn(List<DatasEntity> enemy_spawn_data)
    {
        foreach (DatasEntity dat in enemy_spawn_data) {
            yield return new WaitForSeconds(dat.Time); //predelay
            StartCoroutine(spawnGroup(dat));
        }
    }
    IEnumerator spawnGroup(DatasEntity dat)
    {
        for (int i = 0; i < dat.Repeat; i++)
        {
            GameObject enemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/" + dat.EnemyKey));
            //random range
            Vector2 offset = Random.insideUnitCircle * dat.SpRange;
            Vector3 p = spawnPoints[dat.Start].position;
            enemy.transform.position = new Vector3(p.x + offset.x, p.y + offset.y, 0);
            yield return new WaitForSeconds(dat.Interval); //interval
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
