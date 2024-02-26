using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveDir;
    float speed = 10;
    float acceleration = 7;
    float deceleration = 10;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckMoveInput();
    }

    void FixedUpdate()
    {
        Vector2 velocityInput = moveDir * speed;
        Vector2 velocityDiff = velocityInput - rb.velocity;
        float accelRate = velocityInput != Vector2.zero ? acceleration : deceleration;
        Vector2 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }

    void CheckMoveInput()
    {
        Vector3 inputDir = Vector3.zero;

        if(Input.GetKey(KeyCode.W))
            inputDir += Vector3.up;
        if(Input.GetKey(KeyCode.S))
            inputDir -= Vector3.up;
        if(Input.GetKey(KeyCode.D))
            inputDir += Vector3.right;
        if(Input.GetKey(KeyCode.A))
            inputDir -= Vector3.right;

        moveDir = inputDir.normalized;
    }
}
