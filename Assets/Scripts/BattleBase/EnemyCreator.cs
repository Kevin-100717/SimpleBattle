using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    public static EnemyCreator instance;
    public List<EnemyWaveData> waves = new List<EnemyWaveData>();
    public int currentWave = 0;
    public int currentEnemy = 0;
    public float timer = 0;
    public bool flag = false;
    private bool f = true;
    public List<GameObject> specialTraps = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        foreach (GameObject trap in waves[currentWave].traps)
        {
            trap.SetActive(true);
        }
        StartCoroutine(showInfo());
    }
    IEnumerator showInfo()
    {
        yield return null;
        UIManager.instance.ShowMessage(new Msg
        {
            title = "����",
            eng = "Mission",
            content = "������е���\n��ǰ������1/" + waves.Count
        });
    }

    // Update is called once per frame
    void Update()
    {
        List<EnemyWaveData.EnemyData> wave = waves[currentWave].enemies;
        if (currentEnemy >= wave.Count && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            if (currentWave == waves.Count - 1)
            {
                UIManager.instance.ShowMessage(new Msg
                {
                    title = "�ɹ�",
                    eng = "FINISH",
                    content = "��������ɹ����������"
                });
                UIManager.instance.End();
                Destroy(gameObject);
            }
            if (f)
            {
                if (waves[currentWave].bar != null) waves[currentWave].bar.SetActive(false);
                f = false;
            }
            if (!flag) return;
            f = true;
            flag = false;
            foreach (GameObject trap in waves[currentWave].traps)
            {
                trap.SetActive(false);
            }
            timer = 0;
            currentEnemy = 0;
            currentWave++;
            UIManager.instance.ShowMessage(new Msg
            {
                title = "����",
                eng = "Mission",
                content = currentWave == waves.Count-1 ? "BOSS��������\n��ǰ������" + (currentWave + 1) + "/" + waves.Count : "������е���\n��ǰ������" + (currentWave + 1) + "/" + waves.Count
            });
            foreach (GameObject trap in waves[currentWave].traps)
            {
                trap.SetActive(true);
            }
            return;
        }
        if (currentEnemy >= wave.Count) return;
        timer += Time.deltaTime;
        if (wave[currentEnemy].time <= timer)
        {
            wave[currentEnemy].createPoint.AddComponent<EnemyCreate>().Init(wave[currentEnemy].enemyPrefab, wave[currentEnemy].loop, wave[currentEnemy].interval);
            currentEnemy++;
        }
    }
}
