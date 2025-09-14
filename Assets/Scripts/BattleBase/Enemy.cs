using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int hp = 5;
    private int hpTotal;
    public GameObject bloodPrefab;
    public CanvasGroup hpBar;
    public Transform hpWhite;
    public Transform hpRed;
    private bool hpDisplay = false;
    private Transform target;
    private NavMeshAgent agent;
    public float speed = 2f;
    public float atkBefore = 0.4f;
    public GameObject atkWarn;
    public float atkInterval = 1.5f;
    private float atkTimer = 0f;
    public float atkRange = 6.5f;
    public GameObject bulletPrefab;
    [HideInInspector]
    public bool isDie;
    public List<Item> dropItem;
    public GameObject itemBallPrefab;
    public int damage = 2;
    public bool control = true;
    public bool hpLock = false;
    public float knockbackDistance = 2.5f; // 敌人被击退的距离

    private bool isKnockback = false;
    private Vector3 knockbackTarget;
    private float knockbackSpeed = 18f;
    private float knockbackTimer = 0f;
    private float knockbackDuration = 0.07f; // 击退持续时间
    private Vector3 knockbackDir;
    // Start is called before the first frame update
    void Start()
    {
        hpTotal = hp;
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (!control) return;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        atkTimer = atkInterval;
    }
    public enum AttackSource
{
    Sword,
    Bullet,
    Other
}

    public void TakeDamage(int damage, AttackSource source = AttackSource.Other)
    {
        if(isDie || hpLock)
        {
            return;
        }
        // 只有非boss(control==true)且被sword攻击才会被击退
        if (control && !isKnockback && source == AttackSource.Sword)
        {
            Transform player = GameObject.FindGameObjectWithTag("Player").transform;
            knockbackDir = (transform.position - player.position).normalized;
            isKnockback = true;
            knockbackTimer = 0f;
            if (agent != null) agent.isStopped = true;
        }
        if (hp-damage <= 0)
        {
            GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
            GetComponentInChildren<ParticleSystem>().Play();
            isDie = true;
            foreach(Item item in dropItem)
            {
                GameObject ball = Instantiate(itemBallPrefab, transform.position, Quaternion.identity);
                ball.GetComponent<ItemBall>().item = item;
            }
            Destroy(gameObject,0.5f);
            return;
        }
        if (hpDisplay)
        {
            CancelInvoke("HideBar");
            DOTween.Kill(hpBar);
        }
        hp -= damage;
        GameObject blood = Instantiate(bloodPrefab, transform.Find("Canvas"));
        blood.GetComponent<Blood>().dmg = damage;
        hpBar.DOFade(1, 0);
        hpRed.DOScaleX((float)hp / hpTotal, 0.1f);
        hpWhite.DOScaleX((float)hp / hpTotal, 0.4f);
        hpDisplay = true;
        Invoke("HideBar", 0.6f);

    }
    private void HideBar()
    {
        hpBar.DOFade(0, 0.4f).OnComplete(() => {
            hpDisplay = false;
        });
    }
    // Update is called once per frame
    void Update()
    {
        if (isDie)
        {
            return;
        }
        // 处理击退逻辑，击退指定时间
        if (isKnockback)
        {
            transform.position += knockbackDir * knockbackSpeed * Time.deltaTime;
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= knockbackDuration)
            {
                isKnockback = false;
                if (agent != null) agent.isStopped = false;
            }
            // 击退时不执行导航和攻击
            return;
        }
        if (!control)
        {
            return;
        }
        agent.SetDestination(target.position);
        if (Vector3.Distance(transform.position,target.position) < atkRange)
        {
            agent.isStopped = true;
            if(atkTimer >= atkInterval)
            {
                atkTimer = 0;
                atkWarn.SetActive(true);
                Invoke("startAtk", atkBefore);
            }
        }
        else
        {
            agent.isStopped = false;
        }
        atkTimer += Time.deltaTime;
    }
    private void startAtk()
    {
        atkTimer = 0;
        atkWarn.SetActive(false);
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().damage = damage;
    }
    private void OnDestroy()
    {
        if(Cursor.instance.GetEnemyList().IndexOf(gameObject) >= 0)
        {
            Cursor.instance.RemoveTarget(gameObject);
        }
    }
}
