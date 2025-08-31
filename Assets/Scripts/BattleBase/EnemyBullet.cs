using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public float speed = 5f;
    private Vector3 direction;
    public int damage = 2;
    public float offset;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        direction = (target.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards the player
        // Move the bullet in that direction
        transform.position += direction * speed * Time.deltaTime;
        // Optional: Rotate the bullet to face the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+offset));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall"))
        {
            if(collision.CompareTag("Player")) collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Die();
        }
    }
    public void Die()
    {
        speed = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        try
        {
            transform.Find("Tri").gameObject.GetComponent<ParticleSystem>().Stop();
            transform.Find("Bomb").gameObject.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject, 0.5f);
        }
        catch
        {
            Destroy(gameObject);
        }
        
    }
}
