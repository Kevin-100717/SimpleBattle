using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Transform dayProgressBar;
    public Text dayNumText;
    public GameObject enemySpawnIconPrefab;
    public RectTransform dayEventFrame;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    public void SetDay(int day,float time,float total)
    {
        dayNumText.text = $"Day {day}";
        dayProgressBar.transform.localScale = new Vector3(time / total, 1, 1);
    }
    public void SetEvent(DaysEntity day)
    {
        foreach (Transform child in dayEventFrame)
        {
            Destroy(child.gameObject);
        }
        foreach (EventEntity e in day.Event)
        {
            if(e.Type == "enemy_spawn")
            {
                GameObject icon = Instantiate(enemySpawnIconPrefab, dayEventFrame);
                //Debug.Log((float)e.Time / (float)day.Time);
                icon.transform.localPosition = new Vector3(((float)e.Time / (float)day.Time) * dayEventFrame.sizeDelta.x, 0, 0);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
