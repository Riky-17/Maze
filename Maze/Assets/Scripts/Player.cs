using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject signal;
    public static event Action<int> onSignalUpdate;
    Rigidbody2D rb;
    Vector2 moveDir;
    float speed = 10;
    float acceleration = 7;
    float deceleration = 10;
    float rotationSpeed = 6;
    float RotationVelocity => rotationSpeed * 90; //90 stands for the degrees
    int signalsAmount;
    bool isPlayerInsideMaze = false;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        signalsAmount = (int)(Mathf.Round(GameManager.Instance.MazeHeight * GameManager.Instance.MazeWidth / 50f / 5) * 5);
        signalsAmount = Mathf.Clamp(signalsAmount, 3, 50);
    }

    void OnEnable() => MazeGenerator.onPlayerInside += SetPlayerInside;
    void OnDisable() => MazeGenerator.onPlayerInside -= SetPlayerInside;

    void Start()
    {
        onSignalUpdate?.Invoke(signalsAmount);
    }

    void Update()
    {
        CheckMoveInput();
        if(moveDir != Vector2.zero)
            RotatePlayer();
        if(Input.GetKeyDown(KeyCode.Space) && isPlayerInsideMaze)
            PlaceSignal();
    }

    void FixedUpdate()
    {
        Vector2 velocityInput = moveDir * speed;
        Vector2 velocityDiff = velocityInput - rb.velocity;
        float accelRate = velocityInput != Vector2.zero ? acceleration : deceleration;
        Vector2 force = velocityDiff * accelRate;
        rb.AddForce(force);
    }

    void PlaceSignal()
    {
        if(signalsAmount <= 0)
            return;

        Instantiate(signal, new Vector3(0, 0, .1f) + transform.position, Quaternion.identity);
        signalsAmount--;
        onSignalUpdate?.Invoke(signalsAmount);
    }

    void RotatePlayer()
    {
        Quaternion lookDir = Quaternion.LookRotation(transform.forward, moveDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookDir, RotationVelocity * Time.deltaTime);
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

    void SetPlayerInside() => isPlayerInsideMaze = true;
}
