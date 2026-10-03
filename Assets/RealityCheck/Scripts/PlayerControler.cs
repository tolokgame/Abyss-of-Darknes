using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private void Update()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.A))
            horizontal = -1f;

        if (Input.GetKey(KeyCode.D))
            horizontal = 1f;

        if (Input.GetKey(KeyCode.S))
            vertical = -1f;

        if (Input.GetKey(KeyCode.W))
            vertical = 1f;

        Vector3 movement = new Vector3(
            horizontal,
            0f,
            vertical
        ).normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}