using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBall : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform target;
    public float speed = 15f;
    public Item item;
    private bool flag = false;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        float knockRange = Random.Range(1f, 3f);
        float angle = Random.Range(0, 360);
        Vector3 dir = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        transform.DOMove(dir * knockRange + transform.position, 0.5f).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            flag = true;
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (!flag) return;
        //œÚtarget“∆∂Ø
        Vector3 dir = (target.position - transform.position).normalized;
        transform.position += dir * Time.deltaTime * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().GetItem(item);
            Destroy(gameObject);
        }
    }
}
