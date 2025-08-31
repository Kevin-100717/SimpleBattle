using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blood : MonoBehaviour
{
    public int dmg = 1;
    public Text dmgTxt;
    public float range = 200;
    // Start is called before the first frame update
    void Start()
    {
        //以自己为中心，随机一个范围内的点
        Vector2 randomPos = Random.insideUnitCircle * range;
        transform.localPosition = new Vector3(randomPos.x, randomPos.y, 0);
        dmgTxt.transform.DOLocalMoveY(transform.localPosition.y + 200, 0.7f);
        dmgTxt.DOFade(0.5f,0.7f).OnComplete(() => {
            Destroy(gameObject);
        });
    }

    // Update is called once per frame
    void Update()
    {
        dmgTxt.text = "-" + dmg.ToString();
    }
}
