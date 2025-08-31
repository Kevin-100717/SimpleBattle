using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    public static EffectController instance;
    public Player player;
    public List<BuffTemplate> effects = new List<BuffTemplate>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        player = FindObjectOfType<Player>();
    }
    public void ApplyEffect(BuffTemplate buff)
    {
        switch (buff.buff.buffType)
        {
            case Buff.BuffType.DmgAtk:
                player.dmg += (int)buff.buff.factor;
                break;
            case Buff.BuffType.DmgShoot:
                player.bulletDamage += (int)buff.buff.factor;
                break;
            case Buff.BuffType.Hp:
                player.hp += (int)buff.buff.factor;
                player.hp = Mathf.Min(player.hp, player.hpTotal);
                UIManager.instance.SetHpBar(player.hp, player.hpTotal);
                break;
            case Buff.BuffType.ShootSpeed:
                player.shootRate = Mathf.Max(0.05f, player.shootRate + buff.buff.factor);
                break;
            case Buff.BuffType.Def:
                player.def += buff.buff.factor;
                break;
            case Buff.BuffType.LookRange:
                Cursor.instance.range += buff.buff.factor;
                break;
            case Buff.BuffType.BulletNum:
                player.bulletNumMax += (int)buff.buff.factor;
                player.totalBulletNum += (int)buff.buff.factor;
                Cursor.instance.SetBullet(player.totalBulletNum, player.bulletNumMax);
                break;
            case Buff.BuffType.BulletPreShoot:
                player.bulletNumPreShoot += (int)buff.buff.factor;
                player.bulletNumPreShoot = Mathf.Min(player.bulletNumPreShoot, 10);
                break;
        }
        if (!buff.buff.once)
        {
            effects.Add(buff);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
