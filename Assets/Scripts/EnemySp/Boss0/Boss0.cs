using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss0 : MonoBehaviour
{
    private Animator animator;
    private Enemy enemyController;
    private SkillController skillController;
    public GameObject enemyChild;
    private int trapIndex = 0;
    public GameObject bombPrefab;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyController = GetComponent<Enemy>();
        skillController = GetComponent<SkillController>();
    }
    public void ResetTrigger(string x)
    {
        animator.ResetTrigger("a" + x);
        skillController.onSkill = false;
        switch (x)
        {
            case "1":
                enemyController.hpLock = false;
                break;
        }
    }
    public void Skill0()
    {
        if (trapIndex == EnemyCreator.instance.specialTraps.Count) return;
        EnemyCreator.instance.specialTraps[trapIndex].SetActive(true);
        trapIndex++;

    }
    public void Skill1()
    {
        Instantiate(enemyChild, transform.position, Quaternion.identity);
    }
    public void Skill2()
    {
        for(int i=0;i<3;i++)
        {
            Instantiate(bombPrefab, transform.position, Quaternion.identity);
        }
    }
    public void TriggerSkill(string x)
    {
        animator.SetTrigger("a" + x);
        switch (x)
        {
            case "1":
                enemyController.hpLock = true;
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
}
