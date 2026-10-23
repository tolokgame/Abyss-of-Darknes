using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 5f;

    [Header("Mouse")]
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;

    [Header("Ground Check")]
    public LayerMask groundMask;
    public float groundCheckDistance = 1.2f;

    private CharacterController controller;
    private PlayerState playerState;
    private Vector3 velocity;
    private float xRotation = 0f;
    private float standingHeight;
    private Vector3 standingCenter;
    private bool isCrouching;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerState = GetComponent<PlayerState>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        standingHeight = controller.height;
        standingCenter = controller.center;
    }

    private void Update()
    {
        // Якщо гравець НЕ схований — дозволяємо рухати камерою
        if (playerState == null || !playerState.IsHidden)
        {
            Look();
        }

        Move();
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void Move()
    {
        if (playerState != null && playerState.IsHidden)
        {
            velocity.x = 0f;
            velocity.z = 0f;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        bool isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // Логіка присідання: утримуємо кнопку LeftControl (можна змінити на свій варіант)
        isCrouching = Input.GetKey(KeyCode.LeftControl);

        // Зміна швидкості залежно від стану
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Стрибок (дозволено лише якщо не присіли)
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Розрахунок цільової висоти
        float targetHeight = isCrouching ? crouchHeight : standingHeight;

        // Плавна зміна висоти (використовуємо MoveTowards для стабільної швидкості)
        controller.height = Mathf.MoveTowards(controller.height, targetHeight, crouchSpeed * Time.deltaTime);

        // Коригування центру коллайдера
        controller.center = new Vector3(
            standingCenter.x,
            standingCenter.y - (standingHeight - controller.height) / 2f,
            standingCenter.z
        );

        // Оновлення позиції камери (щоб вона опускалася разом із присіданням)
        if (cameraHolder != null)
        {
            // Припускаємо, що початкова висота камери дорівнює standingHeight - 0.2f (підлаштуйте під себе)
            float cameraY = controller.height - 0.2f;
            cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraY, cameraHolder.localPosition.z);
        }

        // Гравітація та рух по Y
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

}







/*using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Mouse")]
    public Transform cameraHolder;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
        Move();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}*/