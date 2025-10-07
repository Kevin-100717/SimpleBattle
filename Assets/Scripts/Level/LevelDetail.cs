using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDetail : MonoBehaviour
{
    public static LevelDetail Instance;
    public Text title;
    public Text difficult;
    public Text desc;
    public string sceneName;
    public List<Color> difficults = new List<Color>();
    private bool show = false;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        GetComponent<CanvasGroup>().DOFade(0, 0);
        gameObject.SetActive(false);
    }
    public void ShowDetail(string title,int difficult,string desc,string sceneName)
    {
        show = true;
        transform.DOLocalMoveX(-766.6f, 0.5f);
        GetComponent<CanvasGroup>().DOFade(1, 0.5f);
        this.title.text = title;
        this.difficult.text = "难度:"+(new List<string>() { "简单", "一般", "困难", "不可能" })[difficult];
        this.difficult.color = difficults[difficult];
        this.desc.text = desc;
        this.sceneName = sceneName;
    }
    public void HideDetail()
    {
        transform.DOLocalMoveX(179.34f, 0.5f);
        GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && show)
        {
            show = false;
            HideDetail();
        }
    }
    public void OnLevelClick()
    {
        HideDetail();
        Debug.Log(sceneName);
        LevelLoader.Instance.LoadLevel(sceneName);
    }
}
