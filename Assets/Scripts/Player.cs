using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    Vector2 movement;

    void Update()
    {
        if (!GameController.instance.startFlag) return;
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D 或 左/右
        float moveY = Input.GetAxisRaw("Vertical");   // W/S 或 上/下
        movement = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        if (!GameController.instance.startFlag) return;
        if (rb != null)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
