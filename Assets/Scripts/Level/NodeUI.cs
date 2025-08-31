using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public string title;
    public int difficult;
    public string content;
    public string sceneId;
    public GameObject messageBoxPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ShowMessage()
    {
        GameObject messageBox = Instantiate(messageBoxPrefab,transform.parent.parent);
        messageBox.GetComponent<MessageBox>().title = title;
        messageBox.GetComponent<MessageBox>().content = content;
    }
    public void ShowLevel()
    {
        LevelDetail.Instance.gameObject.SetActive(true);
        LevelDetail.Instance.ShowDetail(title, difficult, content, sceneId);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
