using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform player;
    float speed = 5;

    void FixedUpdate()
    {
        Vector3 playerPos = new(player.position.x, player.position.y, transform.position.z);
        if((playerPos - transform.position).magnitude > .5f)
        {
            transform.position = Vector3.Slerp(transform.position, playerPos, speed * Time.deltaTime);
        }
    }
}
