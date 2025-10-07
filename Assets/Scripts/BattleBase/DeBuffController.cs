using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffController : MonoBehaviour
{
    public List<Buff> debuffs;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }
    private bool haveBuff(Buff buff)
    {
        bool flag = false;
        foreach (Buff b in debuffs)
        {
            if(b.buffType == buff.buffType)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }
    public void AddDebuff(Buff buff)
    {
        if (haveBuff(buff)) { return; }
        debuffs.Add(buff);
        switch (buff.buffType)
        {
            case Buff.BuffType.DeShoot:
                buff.counter = player.bulletDamage;
                player.bulletDamage -= 1;
                break;
            case Buff.BuffType.DeSpeed:
                buff.counter = player.moveSpeed;
                player.moveSpeed /= 2;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        List<Buff> removedBuff = new List<Buff>();
        foreach (Buff buff in debuffs) {
            switch (buff.buffType)
            {
                case Buff.BuffType.Du:
                    buff.counter += Time.deltaTime;
                    if (buff.counter > 0.5f) {
                        buff.counter = 0;
                        player.TakeDamage(1, true);
                    }
                    break;
            }
            buff.time -= Time.deltaTime;
            if(buff.time < 0)
            {
                buff.time = 0;
                switch (buff.buffType)
                {
                    case Buff.BuffType.DeShoot:
                        player.bulletDamage = (int)buff.counter;
                        break;
                    case Buff.BuffType.DeSpeed:
                        player.moveSpeed = (int)buff.counter;
                        break;
                }
                removedBuff.Add(buff);
            }
        }
        foreach(Buff rb in removedBuff)
        {
            debuffs.Remove(rb);
        }
    }
}
