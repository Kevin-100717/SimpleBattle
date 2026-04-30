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