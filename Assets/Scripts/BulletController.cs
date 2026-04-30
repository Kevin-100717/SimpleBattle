using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    public Damage damage;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            HandleHit();
        } else if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy e = collision.gameObject.GetComponent<Enemy>();
            e.TakeDamage(damage);
            HandleHit();
        }
    }
    void HandleHit()
    {
        speed = 0; // 停止移动
        Transform hitEffect = transform.Find("Hit");
        if (hitEffect != null)
        {
            // 隐藏SpriteRenderer
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;

            // 关闭Collider2D
            var collider = GetComponent<Collider2D>();
            if (collider != null)
                collider.enabled = false;

            // 确保Hit激活
            hitEffect.gameObject.SetActive(true);

            // 播放Hit特效
            var ps = hitEffect.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                ps.Play();
                StartCoroutine(DestroyAfterParticle(ps));
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        System.Collections.IEnumerator DestroyAfterParticle(ParticleSystem ps)
        {
            yield return new WaitWhile(() => ps.isPlaying);
            Destroy(gameObject);
        }
    }
}
