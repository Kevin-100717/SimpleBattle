using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public float speed = 5f;
    public Vector3 direction; // 改为public，便于调试
    public int damage = 2;
    public float offset;
    private bool isReflected = false;
    public bool useCustomDirection = false;
    void Start()
    {
        if (!useCustomDirection || direction == Vector3.zero)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            direction = (target.position - transform.position).normalized;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards the player or反射方向
        transform.position += direction * speed * Time.deltaTime;
        // Optional: Rotate the bullet to face the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+offset));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isReflected && collision.gameObject.CompareTag("PlayerSword"))
        {
            // 反射逻辑
            isReflected = true;
            // 获取鼠标世界坐标
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            direction = (mouseWorld - transform.position).normalized;
            // 可选：更换tag，避免再次被sword反射
            gameObject.tag = "Untagged";
            return;
        }
        if (!isReflected && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall")))
        {
            if(collision.CompareTag("Player")) collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            Die();
        }
        else if (isReflected && collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null && !enemy.isDie)
            {
                enemy.TakeDamage(damage, Enemy.AttackSource.Bullet);
            }
            Die();
        }
        else if (isReflected && collision.gameObject.CompareTag("Wall"))
        {
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
    public void ReflectToMouse()
    {
        if (isReflected) return;
        isReflected = true;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0;
        direction = (mouseWorld - transform.position).normalized;
        gameObject.tag = "Untagged";
    }
    public void SetDirection(Vector3 dir)
    {
        useCustomDirection = true;
        direction = dir.normalized;
        // 立即调整朝向
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
    }
}
