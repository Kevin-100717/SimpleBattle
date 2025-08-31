using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBox : MonoBehaviour
{
    public string title;
    public string content;
    public Text title_text;
    public Text content_text;
    private float speedPreChar = 0.05f;
    private string textShow;
    private int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        title_text.text = title;
        StartCoroutine(showTextPreChar());
    }
    public void close()
    {
        Destroy(gameObject);
    }
    IEnumerator showTextPreChar()
    {
        while (index < content.Length)
        {
            textShow += content[index];
            content_text.text = textShow;
            index++;
            yield return new WaitForSeconds(speedPreChar);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
