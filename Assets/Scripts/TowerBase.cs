using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : MonoBehaviour
{
    public CanvasGroup statusBar;
    // Start is called before the first frame update
    void Start()
    {
        statusBar.DOFade(0, 0);
    }
    public void ShowStatus()
    {
        statusBar.DOFade(1, 0.15f);
    }
    public void HideStatus()
    {
        statusBar.DOFade(0, 0.15f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
