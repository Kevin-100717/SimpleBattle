using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Msg
{
    public string title;
    public string eng;
    public string content;
}
public class Message : MonoBehaviour
{
    public Msg msg;
    public Text title;
    public Text eng;
    public Text content;
    // Start is called before the first frame update
    void Start()
    {
        title.text = msg.title;
        eng.text = msg.eng;
        content.text = msg.content;
        transform.localPosition = new Vector3(972, 434.152f,0);
        transform.DOLocalMoveX(282, 0.8f);
        Invoke("Hide", 3.5f);
    }
    private void Hide()
    {
        transform.DOLocalMoveX(972, 0.8f).OnComplete(() => {
            Destroy(gameObject,0.3f);
        });
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
