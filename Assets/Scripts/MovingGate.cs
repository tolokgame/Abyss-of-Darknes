using System.Collections;
using UnityEngine;

public class MovingGate : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private Vector3 openOffset = new Vector3(0, 4f, 0); // На скільки підняти ворота
    [SerializeField] private float moveSpeed = 2f; // Швидкість руху
    [SerializeField] private float closeDelay = 5f; // Час очікування перед автоматичним закриттям

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isMoving = false;
    private InteractionButton currentButton; // Посилання на кнопку, яка запустила процес

    private void Start()
    {
        // Запам'ятовуємо початкову позицію як "закриту"
        closedPosition = transform.position;
        // Обчислюємо кінцеву позицію "відкритих" воріт
        openPosition = closedPosition + openOffset;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    // Метод викликається кнопкою, передаючи посилання на саму себе (this)
    public void TriggerGate(InteractionButton button)
    {
        if (!isMoving)
        {
            currentButton = button;
            StartCoroutine(GateSequence());
        }
    }

    private IEnumerator GateSequence()
    {
        isMoving = true;

        // --- 1. ВІДКРИВАННЯ ---
        PlaySound(openSound);
        while (Vector3.Distance(transform.position, openPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, openPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = openPosition;

        // --- 2. ОЧІКУВАННЯ ---
        yield return new WaitForSeconds(closeDelay);

        // --- 3. ЗАКРИВАННЯ ---
        PlaySound(closeSound);
        while (Vector3.Distance(transform.position, closedPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, closedPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = closedPosition;

        // --- 4. ЗАВЕРШЕННЯ ЦИКЛУ ---
        isMoving = false;
        
        // Повертаємо кнопку в початковий зелений стан точно в момент закриття воріт
        if (currentButton != null)
        {
            currentButton.ResetButton();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
