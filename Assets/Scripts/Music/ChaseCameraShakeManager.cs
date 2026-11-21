using UnityEngine;

public class ChaseCameraShakeManager : MonoBehaviour
{
    public static ChaseCameraShakeManager Instance;

    [Header("Link")]
    [Tooltip("Drag camera here")]
    [SerializeField] private Transform cameraTransform;

    [Header("Interval Settings Timmings")]
    [SerializeField] private float delayBeforeFade = 5f; // 5 секунд очікування після погоні
    [SerializeField] private float fadeOutSpeed = 0.5f;  // Швидкість затухання тряски
    [SerializeField] private float fadeInSpeed = 2f;     // Швидкість наростання тряски

    [Header("Shake Setings")]
    [SerializeField] private float maxMagnitude = 0.15f; // Максимальна сила зміщення камери
    [SerializeField] private float shakeFrequency = 25f; // Частота вібрації (швидкість тремтіння)
    [SerializeField] private float maxRotationAngle = 2f;// Максимальний нахил камери по осі Z (0 = вимкнено)

    private int activeChasers = 0;       // Скільки ворогів зараз женуться
    private float delayTimer = 0f;       // Таймер очікування
    private float currentIntensity = 0f; // Поточна інтенсивність (від 0 до 1, аналог volume)
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;

    private void Awake()
    {
        // Singleton для зручного доступу з ворогів
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Якщо забули перетягнути камеру в інспекторі, беремо Main Camera автоматично
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        if (cameraTransform != null)
        {
            originalLocalPos = cameraTransform.localPosition;
            originalLocalRot = cameraTransform.localRotation;
        }
    }

    private void Update()
    {
        if (cameraTransform == null) return;

        // 1. Логіка зміни інтенсивності (ідентична до вашої гучності)
        if (activeChasers > 0)
        {
            delayTimer = delayBeforeFade;
            currentIntensity = Mathf.MoveTowards(currentIntensity, 1f, fadeInSpeed * Time.deltaTime);
        }
        else if (currentIntensity > 0f)
        {
            if (delayTimer > 0f)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                currentIntensity = Mathf.MoveTowards(currentIntensity, 0f, fadeOutSpeed * Time.deltaTime);
            }
        }

        // 2. Застосування тряски до камери
        if (currentIntensity > 0f)
        {
            ApplyShake();
        }
        else
        {
            // Повертаємо камеру в ідеально рівне положення, коли тряска закінчилась
            cameraTransform.localPosition = originalLocalPos;
            cameraTransform.localRotation = originalLocalRot;
        }
    }

    private void ApplyShake()
    {
        // Використовуємо PerlinNoise для плавної та реалістичної вібрації
        float time = Time.time * shakeFrequency;
        float noiseX = (Mathf.PerlinNoise(time, 0f) * 2f) - 1f;
        float noiseY = (Mathf.PerlinNoise(0f, time) * 2f) - 1f;
        float noiseRot = (Mathf.PerlinNoise(time, time) * 2f) - 1f;

        // Множимо на currentIntensity для плавного входу та виходу
        float currentMag = maxMagnitude * currentIntensity;
        Vector3 offset = new Vector3(noiseX * currentMag, noiseY * currentMag, 0f);
        cameraTransform.localPosition = originalLocalPos + offset;

        // Додаємо легкий нахил камери, якщо вказано кут > 0
        if (maxRotationAngle > 0f)
        {
            float currentRot = maxRotationAngle * currentIntensity * noiseRot;
            cameraTransform.localRotation = originalLocalRot * Quaternion.Euler(0f, 0f, currentRot);
        }
    }

    // Ворог викликає це, коли входить у ChaseState
    public void StartChase()
    {
        activeChasers++;
    }

    // Ворог викликає це, коли виходить із ChaseState
    public void StopChase()
    {
        activeChasers = Mathf.Max(0, activeChasers - 1);
    }
}
