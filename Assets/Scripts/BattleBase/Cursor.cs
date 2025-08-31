using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    public static Cursor instance;
    private List<GameObject> enemyList = new List<GameObject>();
    public float range = 3f;
    public float followSpeed = 15f;
    public Text bulletText;
    public Image circleBulletBar;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }
    public List<GameObject> GetEnemyList()
    {
        return enemyList;
    }
    // Update is called once per frame
    void Update()
    {
        float radio = range / 3f;
        transform.localScale = new Vector3(radio, radio, 1);
        //�������
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane; // ���� z �����
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // ���� z ��Ϊ 0
        transform.position = Vector3.Lerp(transform.position, worldPosition, followSpeed * Time.deltaTime);
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if(enemy == null)
            {
                continue; // Skip if the enemy is null
            }
            if(enemy.GetComponent<Enemy>().isDie)
            {
                continue; // Skip if the enemy is dead
            }
            if (Vector3.Distance(transform.position, enemy.transform.position) <= range)
            {
                if (!enemyList.Contains(enemy))
                {
                    enemyList.Add(enemy);
                }
            }
            else
            {
                if (enemyList.Contains(enemy))
                {
                    enemyList.Remove(enemy);
                }
            }
        }
    }
    public void SetBullet(int num,int total)
    {
        //�����ӵ�������ԲȦ��
        bulletText.text = num.ToString() + " / " + total.ToString();
        float fillAmount = (float)num / total;
        circleBulletBar.fillAmount = fillAmount;
    }
    public void RemoveTarget(GameObject e)
    {
        enemyList.Remove(e);
    }
}
