using System.Collections;
using UnityEngine;

[System.Serializable]
public class Damage
{
    public float value;
    public enum Type
    {
        Physical,
        Magical,
        Real,
    }
    public Type type;
}
[System.Serializable]
public class BuildingsData
{
    public Sprite icon;
    public int cost;
    public GameObject buildingPrefab;
    public string name;
    public Color quality;
    public enum BuildCondition
    {
        NearMainOrHavePower,
        NearPower,
        Anywhere
    }
    public BuildCondition buildCondition;
}
[System.Serializable]
public class BattleInfo
{
    public string num;
    public string name;
    [Multiline(5)]
    public string desc;
    public Color difficulty;
    public string difficultyName;
    public string spNote;
}