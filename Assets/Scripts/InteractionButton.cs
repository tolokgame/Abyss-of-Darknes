using UnityEngine;

public class InteractionButton : MonoBehaviour
{
    [Header("Linked Objects")]
    [SerializeField] private MovingGate targetGate; // Конкретні ворота, прив'язані до цієї кнопки

    [Header("Color Settings")]
    [SerializeField] private Renderer buttonRenderer; // MeshRenderer елемента, що змінює колір
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color pressedColor = Color.red;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pressSound;

    private bool playerInZone = false;
    private bool isPressed = false;

    private void Start()
    {
        // Встановлюємо початковий зелений колір
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = normalColor;
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        // Перевірка: гравець поруч, кнопка активна (не натиснута прямо зараз) і натиснута клавіша E
        if (playerInZone && !isPressed && Input.GetKeyDown(KeyCode.E))
        {
            PressButton();
        }
    }

    private void PressButton()
    {
        isPressed = true;
        
        // Змінюємо колір на червоний
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = pressedColor;
        }

        // Відтворюємо звук натискання кнопки
        if (audioSource != null && pressSound != null)
        {
            audioSource.PlayOneShot(pressSound);
        }

        // Запускаємо ворота і передаємо їм цей скрипт, щоб вони знали, кого скинути в кінці
        if (targetGate != null)
        {
            targetGate.TriggerGate(this);
        }
    }

    // Цей метод викликають самі ворота, коли повністю закриються
    public void ResetButton()
    {
        isPressed = false;
        if (buttonRenderer != null)
        {
            buttonRenderer.material.color = normalColor;
        }
    }

    // Фіксація гравця в тригері кнопки (необхідно налаштувати Collider з галочкою Is Trigger)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
        }
    }
}
