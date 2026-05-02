using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingListChooseBtn : MonoBehaviour
{
    public Image quality;
    public Image icon;
    public BuildingsData bdt;
    public GameObject chooseBlue;
    private bool flag = false;
    // Start is called before the first frame update
    void Start()
    {
        chooseBlue.SetActive(false);
    }
    public void SetUI(BuildingsData bd)
    {
        bdt = bd;
        //setUI
        quality.color = bd.quality;
        icon.sprite = bd.icon;
    }
    public void OnClick()
    {
        flag = !flag;
        if (flag)
        {
            StartController.instance.choosedBuildings.Add(bdt);
        }
        else
        {
            StartController.instance.choosedBuildings.Remove(bdt);
        }
        StartController.instance.UpdateShowChoosedCard();
        chooseBlue.SetActive(flag);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
