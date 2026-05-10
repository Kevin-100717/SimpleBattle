using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlaceBtn : MonoBehaviour
{
    public BuildingsData bdt;
    public Image quality;
    public Text cost;
    public Image icon;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUI(BuildingsData bd)
    {
        bdt = bd;
        bdt.quality.a = 130f / 255f;
        quality.color = bdt.quality;
        cost.text = "COST " + bdt.cost.ToString();
        icon.sprite = bdt.icon;
    }
    public void OnClick()
    {
        GameController.instance.canShoot = false;
        if (GameController.instance.RequireBuild(bdt))
        {
            GameObject g = Instantiate(GameController.instance.buildCursor);
            g.GetComponent<BuildCursor>().bdt = bdt;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
