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
        // 1. ПЕРЕВІРКА ПОВЕРХНІ ТА СКИДАННЯ ГРАВІТАЦІЇ
        bool isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Притискає до землі, щоб не підстрибувати на сходах
        }

        // Якщо гравець сховався
        if (playerState != null && playerState.IsHidden)
        {
            velocity.x = 0f;
            velocity.z = 0f;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        // 2. ОБРОБКА ПРИСІДАННЯ (Зміна висоти капсули)
        isCrouching = Input.GetKey(KeyCode.LeftControl);
        float targetHeight = isCrouching ? crouchHeight : standingHeight;

        // Плавно змінюємо висоту CharacterController
        controller.height = Mathf.MoveTowards(controller.height, targetHeight, crouchSpeed * Time.deltaTime);

        // ВАЖЛИВО: Для CharacterController присідання має залишати центр нерухомим, 
        // або зміщувати його так, щоб ноги залишалися на землі. Залишаємо базовий центр:
        controller.center = standingCenter;

        // 3. РУХ ПО ГОРИЗОНТАЛІ
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = transform.right * x + transform.forward * z;

        // Створюємо фінальний вектор руху для цього кадру
        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        Vector3 finalMovement = moveDirection * currentSpeed;

        // 4. СТРИБОК
        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. ГРАВІТАЦІЯ (накопичуємо вертикальну швидкість)
        velocity.y += gravity * Time.deltaTime;

        // Об'єднуємо горизонтальну швидкість і вертикальну (гравітацію/стрибок)
        finalMovement.y = velocity.y;

        // ОДИН ЄДИНИЙ ВИКЛИК MOVE НА КАДР (це вирішує проблему польоту вгору і проходження крізь стіни)
        controller.Move(finalMovement * Time.deltaTime);

        // 6. ПОЗИЦІОНУВАННЯ КАМЕРИ
        // Тепер камера плавно слідує за зміною висоти, але жорстко прив'язана до поточної голови капсули
        if (cameraHolder != null)
        {
            // Позиція камери завжди трохи нижче верхньої точки капсули
            float cameraY = (controller.height / 2f) - 0.2f;
            cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraY, cameraHolder.localPosition.z);
        }
    }





    /*private void Move()
    {
        // Комбінуємо перевірку: вбудований isGrounded АБО твій Raycast (про всяк випадок)
        bool isGrounded = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundMask);

        // Головне виправлення: Скидаємо накопичену гравітацію, якщо ми на будь-якій поверхні
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (playerState != null && playerState.IsHidden)
        {
            velocity.x = 0f;
            velocity.z = 0f;
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        isCrouching = Input.GetKey(KeyCode.LeftControl);

        float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        float targetHeight = isCrouching ? crouchHeight : standingHeight;
        controller.height = Mathf.MoveTowards(controller.height, targetHeight, crouchSpeed * Time.deltaTime);

        controller.center = new Vector3(
            standingCenter.x,
            standingCenter.y - (standingHeight - controller.height) / 2f,
            standingCenter.z
        );

        if (cameraHolder != null)
        {
            float cameraY = controller.height - 0.2f;
            cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, cameraY, cameraHolder.localPosition.z);
        }

        // Застосовуємо гравітацію
        velocity.y += gravity * Time.deltaTime;

        // Фінальний рух (включаючи вертикальну швидкість)
        controller.Move(velocity * Time.deltaTime);
    }
    */
}






/*using UnityEngine;

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
*/