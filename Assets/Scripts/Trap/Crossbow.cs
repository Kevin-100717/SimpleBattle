using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : MonoBehaviour
{
    public Transform cd_progressBar;
    public Transform linePa;
    public Transform linePb;
    public Transform linePc;
    public Transform linePd1;
    public Transform linePd2;
    public LineRenderer lineRenderer;
    public GameObject line;
    public float cd = 3f;
    public float cd_timer = 0f;
    public Transform tower;
    private Transform player;
    public Transform shootPoint;
    public GameObject arrowPrefab;
    public int damage = 4;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        linePc.position = linePd1.position;
        linePc.DOMove(linePd2.position, cd);
    }
    // Update is called once per frame
    void Update()
    {
        //tower³¯ÏòÍæ¼Ò
        Vector3 dir = player.position - tower.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        tower.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        cd_timer += Time.deltaTime;
        float ratio = cd_timer / cd;
        cd_progressBar.localScale = new Vector3(ratio, 1, 1);
        if(cd_timer >= cd - 1)
        {
            line.SetActive(true);
        }
        else
        {
            line.SetActive(false);
        }
        if (cd_timer >= cd)
        {
            GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.identity);
            arrow.GetComponent<EnemyBullet>().damage = damage;
            cd_timer = 0;
            linePc.position = linePd1.position;
            linePc.DOMove(linePd2.position, cd);
        }
        lineRenderer.SetPosition(0, linePa.position);
        lineRenderer.SetPosition(1, linePc.position);
        lineRenderer.SetPosition(2, linePb.position);

    }
}
