using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Transform player;
    public float randomRange = 4f;
    public int damage = 5;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //以玩家为中心，随机生成一个位置
        Vector2 randomPos = (Vector2)player.position + Random.insideUnitCircle * randomRange;
        transform.position = randomPos;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void BombAtk()
    {
        if(Vector3.Distance(transform.position,player.position)<=1.25f)
        {
            player.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
    public void End()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
