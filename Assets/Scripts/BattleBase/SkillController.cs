using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class Skill
{
    public float skillCD;
    public float cd;
    public UnityEvent OnSkill;
}
public class SkillController : MonoBehaviour
{
    public List<Skill> skills;
    public bool onSkill = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (onSkill) return;
        foreach(Skill skill in skills)
        {
            skill.cd += Time.deltaTime;
            if(skill.cd > skill.skillCD)
            {
                skill.cd = 0;
                onSkill = true;
                skill.OnSkill.Invoke();
            }
        }
    }
}
