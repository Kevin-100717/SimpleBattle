using UnityEngine;

public class Cursor : MonoBehaviour
{
    public float followSpeed = 10f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!GameController.instance.startFlag) return;
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(mainCamera.transform.position.z); // 适用于正交/透视相机
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = 0f; // 保持在2D平面

        transform.position = Vector3.Lerp(transform.position, mouseWorldPos, followSpeed * Time.deltaTime);
    }
}
