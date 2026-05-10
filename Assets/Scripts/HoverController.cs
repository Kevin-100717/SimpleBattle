using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverController : MonoBehaviour
{
    public UnityEvent OnHover;
    public UnityEvent UnHover;
    private bool hovered;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Hover()
    {
        if (hovered) return;
        hovered = true;
        OnHover.Invoke();
    }
    public void CancelHover()
    {
        if(!hovered) return;
        hovered = false;
        UnHover.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
