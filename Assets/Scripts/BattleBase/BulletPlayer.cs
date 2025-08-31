using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPlayer : MonoBehaviour
{
    public float speed = 18f;
    public float angle = 0;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
        //向angle方向
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    // Update is called once per frame
    void Update()
    {
        //向angle方向移动
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            GameObject enemy = collision.gameObject;
            if (enemy.GetComponent<Enemy>().isDie)
            {
                return;
            }
            enemy.GetComponent<Enemy>().TakeDamage(damage);
            Die();
        }
        if (collision.CompareTag("Wall"))
        {
            Die();
        }
    }
    public void Die()
    {
        speed = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("Tri").gameObject.GetComponent<ParticleSystem>().Stop();
        transform.Find("Bomb").gameObject.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject, 0.5f);
    }
}
