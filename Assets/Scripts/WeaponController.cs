using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Player player;
    public static WeaponController instance;
    [System.Serializable]
    public class Weapon
    {
        public string weaponName;
        public GameObject bulletPrefab;
        public float fireRate = 0.5f;
        public int bulletCount = 30;
        public float reloadPreTime = 0.8f;
        public Damage bulletDamage;
    }
    public List<Weapon> weapons;
    private int currentWeaponIndex = 0;
    private float shootCountdown = 0;
    public int bulletCount;
    private bool isReloading = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = GetComponent<Player>();
        PlayerUIManager.instance.SetupUI();
        bulletCount = GetWeapon().bulletCount;
    }
    public Weapon GetWeapon()
    {
        if (weapons != null && weapons.Count > 0)
        {
            return weapons[currentWeaponIndex];
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.startFlag) return;
        shootCountdown -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shootCountdown <= 0 && !isReloading)
        {
            shootCountdown = GetWeapon().fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (bulletCount <= 0)
        {
            if (!isReloading)
                StartCoroutine(Reload());
            return;
        }

        // 获取鼠标世界坐标
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0;

        // 计算方向
        Vector2 shootDir = (mouseWorldPos - player.transform.position).normalized;

        // 计算旋转角度
        float angle = Mathf.Atan2(shootDir.y, shootDir.x) * Mathf.Rad2Deg;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

        // 实例化子弹并设置朝向
        var bulletPrefab = GetWeapon().bulletPrefab;
        if (bulletPrefab != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, player.transform.position, bulletRotation);
            var bulletCtrl = bullet.GetComponent<BulletController>();
            if (bulletCtrl != null)
            {
                bulletCtrl.SetDirection(shootDir);
                bulletCtrl.damage = GetWeapon().bulletDamage;
            }
        }

        bulletCount--;
        PlayerUIManager.instance.UpdateBulletUI(bulletCount, GetWeapon().bulletCount);

        if (bulletCount <= 0)
        {
            if (!isReloading)
                StartCoroutine(Reload());
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        var weapon = GetWeapon();
        while (bulletCount < weapon.bulletCount)
        {
            yield return new WaitForSeconds(weapon.reloadPreTime);
            bulletCount++;
            PlayerUIManager.instance.UpdateBulletUI(bulletCount,weapon.bulletCount);
        }
        isReloading = false;
    }
}