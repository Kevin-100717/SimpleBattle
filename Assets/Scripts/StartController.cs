using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartController : MonoBehaviour
{
    public static StartController instance;
    public RectTransform buildingListFrame;
    public GameObject buildingChooseBtnPrefab;
    public List<BuildingsData> choosedBuildings;
    public List<GameObject> showChoosedBuildingsImages;
    public CanvasGroup startUIFrame;
    public Sprite emptyIcon;
    public Text battleNameText;
    public Text battleDifficultyText;
    public Text battleSpNoteText;
    public Text battleDescText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        InitBuildingList();
        InitBattleInfo();
    }
    void InitBattleInfo()
    {
        BattleInfo bi = GameRuntimeData.instance.batleInfo;
        battleNameText.text = bi.num + "\n" + bi.name;
        battleDifficultyText.text = bi.difficultyName;
        battleDifficultyText.color = bi.difficulty;
        battleSpNoteText.text = bi.spNote;
        battleDescText.text = bi.desc;
    }
    void InitBuildingList()
    {
        //Clear all child in buildingListFrame
        foreach (Transform child in buildingListFrame)
        {
            Destroy(child.gameObject);
        }
        int i = 0;
        float height = 100;
        foreach(BuildingsData building in GameRuntimeData.instance.buildingsData)
        {
            GameObject btn = Instantiate(buildingChooseBtnPrefab, buildingListFrame);
            BuildingListChooseBtn btnScript = btn.GetComponent<BuildingListChooseBtn>();
            btnScript.SetUI(building);
            i++;
            if(i == 11)
            {
                height += 100;
                i = 0;
            }
        }
        height += 10;
        buildingListFrame.sizeDelta = new Vector2(buildingListFrame.sizeDelta.x, height);
    }
    public void UpdateShowChoosedCard()
    {
        for(int i = 0; i < showChoosedBuildingsImages.Count; i++)
        {
            if(i < choosedBuildings.Count)
            {
                showChoosedBuildingsImages[i].GetComponent<Image>().sprite = choosedBuildings[i].icon;
            }
            else
            {
                showChoosedBuildingsImages[i].GetComponent<Image>().sprite = emptyIcon;
            }
        }
    }
    public void StartBattle()
    {
        startUIFrame.DOFade(0, 0.2f);
        GameController.instance.StartBattle(choosedBuildings);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
