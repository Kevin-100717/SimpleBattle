using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootCD = 1.0f; // �����ȴʱ��
    public int bulletCount = 1;  // ÿ�η�����ӵ���
    public float spreadAngle = 10f; // �෢�ӵ���ɢ��Ƕ�
    public float interval = 0.1f; // ÿ���ӵ�֮��ļ��ʱ��
    private float timer = 0f;
    private bool isShooting = false;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= shootCD && !isShooting)
        {
            StartCoroutine(ShootCoroutine());
            timer = 0f;
        }
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = Random.Range(-spreadAngle / 2f, spreadAngle / 2f) + 90;
            float worldAngle = transform.eulerAngles.z + angle;
            float rad = worldAngle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            var enemyBullet = bullet.GetComponent<EnemyBullet>();
            if (enemyBullet != null)
            {
                enemyBullet.useCustomDirection = true;
                enemyBullet.SetDirection(dir);
            }
            else
            {
                Debug.LogWarning("TowerShoot: bulletPrefab does not have EnemyBullet component!");
            }
            if (i < bulletCount - 1)
                yield return new WaitForSeconds(interval);
        }
        isShooting = false;
    }
}
