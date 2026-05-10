using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCursor : MonoBehaviour
{
    public GameObject buildable;
    public GameObject unbuildable;
    public BuildingsData bdt;
    private bool can_build = false;
    // Start is called before the first frame update
    void Start()
    {
    }
    void SwitchBuild(bool flag)
    {
        if (flag)
        {
            buildable.SetActive(true);
            unbuildable.SetActive(false);
        }
        else
        {
            buildable.SetActive(false);
            unbuildable.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z); // 适用于正交/透视相机
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f; // 保持在2D平面
        transform.position = new Vector3(Mathf.RoundToInt(mouseWorldPos.x), Mathf.RoundToInt(mouseWorldPos.y), 0);
        SwitchBuild(can_build);
    }
}
