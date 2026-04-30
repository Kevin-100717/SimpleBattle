using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public float speed = 3.5f;
    public float health = 100;
    private float totalHp;
    public float defense = 10;
    public float magicDefense = 0;
    public float attackInterval = 2f;
    public float attackDistance = 1.5f;
    public float dashDistance = 1.0f;
    public float dashDuration = 0.2f;
    public float approachRandomRadius = 1.0f; // 敌人靠近时的随机偏移半径

    private NavMeshAgent agent;
    private Transform player;
    private float lastAttackTime;
    private Vector2 randomOffset;
    private bool isAttacking; // 攻击状态标志

    public GameObject hpFrame;
    public Transform hpBarWhite;
    public Transform hpBarRed;

    private bool isDie = false;

    void Start()
    {
        totalHp = health;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        // 每个敌人生成时分配一个随机偏移
        randomOffset = Random.insideUnitCircle * approachRandomRadius;
    }

    void Update()
    {
        if (player == null) return;
        if (isDie) return;

        if (isAttacking)
        {
            agent.isStopped = true;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > attackDistance)
        {
            agent.isStopped = false;
            Vector3 target = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);
            agent.SetDestination(target);
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.position); // 进入攻击距离后直接朝Player移动
            if (Time.time - lastAttackTime > attackInterval)
            {
                lastAttackTime = Time.time;
                StartCoroutine(DashAttack());
            }
        }
    }

    System.Collections.IEnumerator DashAttack()
    {
        // 整个攻击间隔期间停止移动，包含突刺与退回，直到 attackInterval 时间结束
        isAttacking = true;
        agent.isStopped = true;
        float attackStart = Time.time;

        Vector3 startPos = transform.position;
        Vector3 dir = (player.position - transform.position).normalized;
        Vector3 dashPos = transform.position + dir * dashDistance;

        // 向Player方向突刺
        yield return transform.DOMove(dashPos, dashDuration).SetEase(Ease.OutQuad).WaitForCompletion();

        // 伪造造成伤害
        // player.GetComponent<Player>().TakeDamage(攻击力);

        // 退回原位
        yield return transform.DOMove(startPos, dashDuration).SetEase(Ease.InQuad).WaitForCompletion();

        // 等待剩余的攻击间隔时间（保证从开始攻击算起，总共耗时为 attackInterval）
        float elapsed = Time.time - attackStart;
        float remaining = Mathf.Max(0f, attackInterval - elapsed);
        if (remaining > 0f)
            yield return new WaitForSeconds(remaining);

        isAttacking = false;
        agent.isStopped = false;
    }
    public void TakeDamage(Damage dmg)
    {
        if(dmg.type == Damage.Type.Physical)
        {
            health -= Mathf.Max(dmg.value*0.05f, dmg.value - defense);
        }
        else
        {
            health -= Mathf.Max(dmg.value*0.05f, dmg.value - dmg.value*(0.01f*magicDefense));
        }
        if(health > 0)
        {
            UpdateHpBar();
        }
        else
        {
            Die();
        }
    }
    void Die()
    {
        isDie = true;
        GetComponent<Collider2D>().enabled  = false;
        GetComponent<SpriteRenderer>().enabled = false;
        hpFrame.SetActive(false);
        Transform dieEffect = transform.Find("Die");
        var ps = dieEffect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            StartCoroutine(DestroyAfterParticle(ps));
        }
        System.Collections.IEnumerator DestroyAfterParticle(ParticleSystem ps)
        {
            yield return new WaitWhile(() => ps.isPlaying);
            Destroy(gameObject);
        }
    }
    private void UpdateHpBar()
    {
        hpFrame.SetActive(true);
        hpBarWhite.DOScaleX(health / totalHp, 0.5f);
        hpBarRed.DOScaleX(health / totalHp, 0.2f);
    }
}