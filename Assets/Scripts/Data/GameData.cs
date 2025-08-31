using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemType
    {
        Bullet,
        Health,
        Exp
    }
    public ItemType itemType;
    public int num;
}
[System.Serializable]
public class Buff
{
    public enum BuffType
    {
        MoveSpeed,
        AttackSpeed,
        DmgAtk,
        DmgShoot,
        Hp,
        BulletNum,
        LookRange,
        Def,
        ShootSpeed,
        BulletPreShoot,
    }
    public BuffType buffType;
    public float factor;
    public bool once = false;
}
[System.Serializable]
public class BuffTemplate
{
    public string name;
    public Sprite icon;
    [Multiline(5)]
    public string description;
    public Buff buff;
}
[System.Serializable]
public class EnemyWaveData
{
    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemyPrefab;
        public GameObject createPoint;
        public int loop;
        public float interval;
        public float time;
    }
    public List<EnemyData> enemies = new List<EnemyData>();
    public List<GameObject> traps = new List<GameObject>();
    public GameObject bar;
}