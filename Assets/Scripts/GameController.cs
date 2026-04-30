using Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public List<DaysEntity> dayData;
    public int current_day;
    private DaysEntity day;
    private float dayTimer;
    private int eventIndex = -1;
    private bool endFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        dayData = MapLoader.instance.battleData.Days;
        day = dayData[current_day];
        dayTimer = 0;
        eventIndex = -1;
        UIManager.instance.SetEvent(day);
    }
    void TriggerEvent(EventEntity e)
    {
        if(e.Type == "enemy_spawn")
        {
            int dat_index = e.DataIndex;
            List<DatasEntity> spawn_data = MapLoader.instance.battleData.Datas[dat_index];
            EnemySpawner.instance.StartSpawn(spawn_data);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (endFlag) return;
        dayTimer += Time.deltaTime;
        if(eventIndex == -1)
        {
            if (dayTimer >= day.Event[0].Time)
            {
                eventIndex = 0;
                TriggerEvent(day.Event[eventIndex]);
            }
        }
        else
        {
            if(eventIndex < day.Event.Count - 1 && dayTimer >= day.Event[eventIndex + 1].Time)
            {
                eventIndex++;
                TriggerEvent(day.Event[eventIndex]);
            }
        }
            UIManager.instance.SetDay(current_day + 1, dayTimer, day.Time);
        if (dayTimer >= day.Time)
        {
            dayTimer = 0;
            current_day++;
            if (current_day == dayData.Count)
            {
                //Over
                endFlag = true;
                return;
            }
            else
            {
                eventIndex = -1;
                day = dayData[current_day];
                UIManager.instance.SetEvent(day);
            }
        }
    }
}
