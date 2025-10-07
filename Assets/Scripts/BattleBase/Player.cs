using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    private Rigidbody2D rb;
    public enum State
    {
        AttackSword,
        AttackGun,
    }
    public State currentState = State.AttackSword;
    private bool swordAttack = false;
    public GameObject swordPrefab;
    public float swordAttackRate = 0.4f;
    private float swordAttackTimer = 0f;
    public GameObject swordAtkRange;
    private ParticleSystem swordParticle;
    public int dmg = 1;
    private int count = 0;
    public GameObject bulletPrefab;
    public int bulletNumMax = 50;
    public int totalBulletNum = 0;
    public float shootRate = 0.2f;
    private float shootCountdown = 0f;
    public int bulletDamage = 1;
    public int hp = 80;
    public int hpTotal;
    private int exp = 0;
    private int expTotal = 8;
    public int level = 0;
    public float def;
    public int bulletNumPreShoot = 1;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        totalBulletNum = bulletNumMax;
        hpTotal = hp;
    }

    // Update is called once per frame
    void Update()
    {
        AttackController();
        ShootController();
    }
    private void ShootController() {         
        if (Input.GetMouseButton(1))
        {
            if (totalBulletNum > 0 && shootCountdown <= 0f)
            {
                totalBulletNum--;
                shootCountdown = shootRate;
                Debug.Log("Shoot! Bullets left: " + totalBulletNum);
                List<GameObject> el = Cursor.instance.GetEnemyList();
                if (el.Count > 0)
                {
                    GameObject targetEnemy = el[Random.Range(0, el.Count)];
                    Vector3 dir = (targetEnemy.transform.position - transform.position).normalized;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    ShootBullet(angle);
                }
                else
                {
                    //向鼠标方向发射子弹
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 dir = (mousePos - transform.position).normalized;
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    ShootBullet(angle);

                }
                Cursor.instance.SetBullet(totalBulletNum,bulletNumMax);
            }
        }
        shootCountdown -= Time.deltaTime;
    }
    public void ShootBullet(float angle)
    {
        float a = angle - 3 * (bulletNumPreShoot / 2);
        for (int i = 0; i < bulletNumPreShoot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, a+i*3)));
            bullet.GetComponent<BulletPlayer>().angle = a+i*3;
            bullet.transform.position = transform.position;
            bullet.GetComponent<BulletPlayer>().damage = bulletDamage;
        }
    }
    private void AttackController()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (currentState == State.AttackSword)
            {
                if (!swordAttack && swordAttackTimer >= swordAttackRate)
                {
                    swordAttack = true;
                    swordAtkRange.SetActive(true);
                    GameObject swordObj = Instantiate(swordPrefab, transform.Find("Weapon"));
                    //调整刀剑的位置面向鼠标
                    float angle = Mathf.Atan2(Input.mousePosition.y - Camera.main.WorldToScreenPoint(transform.position).y,
                                            Input.mousePosition.x - Camera.main.WorldToScreenPoint(transform.position).x) * Mathf.Rad2Deg;
                    swordAtkRange.transform.rotation = Quaternion.Euler(new Vector3(30, 0, angle));
                    swordAtkRange.GetComponent<PlayerSword>().StartScan();
                    swordParticle = swordObj.GetComponentInChildren<ParticleSystem>();
                    Invoke("SwordAttackController", 0.1f);
                    if (count % 2 == 0)
                    {
                        swordObj.transform.rotation = Quaternion.Euler(new Vector3(30, 0, angle + 65));
                        swordObj.SetActive(true);
                        swordObj.transform.DORotate(new Vector3(30, 15, angle - 65), 0.3f).OnComplete(() =>
                        {
                            swordAtkRange.SetActive(false);
                            Destroy(swordObj);
                            swordAttack = false;
                            swordAttackTimer = 0f;
                        });
                    }
                    else
                    {
                        swordParticle.transform.eulerAngles = new Vector3(0, 0, 180);
                        swordObj.transform.rotation = Quaternion.Euler(new Vector3(30, 0, angle - 65));
                        swordObj.SetActive(true);
                        swordObj.transform.DORotate(new Vector3(30, 15, angle + 65), 0.3f).OnComplete(() =>
                        {
                            swordAtkRange.SetActive(false);
                            Destroy(swordObj);
                            swordAttack = false;
                            swordAttackTimer = 0f;
                        });
                    }
                    count++;
                }
            }
        }
        swordAttackTimer += Time.deltaTime;
    }
    private void SwordAttackController()
    {
        if (swordAttack)
        {
            List<GameObject> enemyList = swordAtkRange.GetComponent<PlayerSword>().GetEnemyList();
            if(enemyList.Count > 0)
            {
                swordParticle.Play();
            }
            foreach (GameObject enemy in enemyList)
            {
                if (enemy != null && enemy.CompareTag("Enemy"))
                {
                    Debug.Log("Enemy -> " + enemy.name);
                    enemy.GetComponent<Enemy>().TakeDamage(dmg, Enemy.AttackSource.Sword);
                }
                else if (enemy != null && enemy.CompareTag("bulletEnemy"))
                {
                    if(enemy.GetComponent<EnemyBullet>().speed < 1) continue;
                    Debug.Log("Bullet -> " + enemy.name);
                    // 反射而不是销毁
                    enemy.GetComponent<EnemyBullet>().ReflectToMouse();
                }
            }
        }
    }
    private void FixedUpdate()
    {
        MoveController();
    }
    void MoveController()
    {
        if (swordAttack) return;
        //移动控制
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        rb.transform.Translate(new Vector3(horizontal,vertical,0) * moveSpeed * Time.fixedDeltaTime);
    }
    public void GetItem(Item item)
    {
        switch (item.itemType)
        {
            case Item.ItemType.Bullet:
                totalBulletNum += item.num;
                if (totalBulletNum > bulletNumMax) totalBulletNum = bulletNumMax;
                Cursor.instance.SetBullet(totalBulletNum, bulletNumMax);
                break;
            case Item.ItemType.Health:
                //加血
                hp += item.num;
                if (hp > hpTotal) hp = hpTotal;
                UIManager.instance.SetHpBar(hp, hpTotal);
                break;
            case Item.ItemType.Exp:
                //加经验
                exp += item.num;
                if (exp >= expTotal)
                {
                    exp = 0;
                    expTotal++;
                    level++;
                    UIManager.instance.LevelUp();
                }
                UIManager.instance.SetLevel(level);
                UIManager.instance.SetExpBar(exp, expTotal);
                break;
        }
    }
    public void TakeDamage(int damage,bool real = false)
    {
        if (real)
        {
            
        }
        else  if (damage <= def)
        {
            damage = 1;
        }
        else
        {
            damage = (int)(damage - def);
        }
        hp -= damage;
        if (hp <= 0)
        {
            //Die();
        }
        UIManager.instance.SetHpBar(hp, hpTotal);
    }
}
