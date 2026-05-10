using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHover : MonoBehaviour
{
    public LayerMask hoverLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameController.instance.startFlag) return;
        List<HoverController> hc = new List<HoverController>();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("HoverControl"))
        {
            hc.Add(g.GetComponent<HoverController>());   
        }
        Vector3 mp = Input.mousePosition;
        Vector3 wp = Camera.main.ScreenToWorldPoint(mp);
        Collider2D hit = Physics2D.OverlapPoint(wp, hoverLayer);
        if (hit != null) {
            hit.gameObject.GetComponent<HoverController>().Hover();
            hc.Remove(hit.gameObject.GetComponent<HoverController>());
        }
        foreach(HoverController h in hc) 
        {
            h.CancelHover();
        }
    }
}
