using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ChaseMusicManager : MonoBehaviour
{
    public static ChaseMusicManager Instance;

    [Header("Налаштування")]
    [SerializeField] private float delayBeforeFade = 5f; // Твої 5 секунд очікування
    [SerializeField] private float fadeOutSpeed = 0.5f;  // Швидкість затухання (0.5 = за 2 сек до нуля)
    [SerializeField] private float fadeInSpeed = 2f;     // Швидкість плавного ввімкнення

    private AudioSource audioSource;
    private int activeChasers = 0; // Скільки ворогів зараз женуться
    private float delayTimer = 0f;

    private void Awake()
    {
        // Робимо Singleton, щоб вороги легко знаходили менеджер
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0f;
    }

    private void Update()
    {
        if (activeChasers > 0)
        {
            // Хтось женеться: оновлюємо таймер і плавно підіймаємо гучність до 1
            delayTimer = delayBeforeFade;

            if (!audioSource.isPlaying)
                audioSource.Play();

            audioSource.volume = Mathf.MoveTowards(audioSource.volume, 1f, fadeInSpeed * Time.deltaTime);
        }
        else if (audioSource.isPlaying)
        {
            // Ніхто не женеться: спочатку чекаємо 5 секунд
            if (delayTimer > 0f)
            {
                delayTimer -= Time.deltaTime;
            }
            else
            {
                // 5 секунд минули: плавно зменшуємо гучність до 0
                audioSource.volume = Mathf.MoveTowards(audioSource.volume, 0f, fadeOutSpeed * Time.deltaTime);

                // Коли дійшло до нуля — зупиняємо трек, щоб скинути його на початок
                if (audioSource.volume <= 0f)
                    audioSource.Stop();
            }
        }
    }

    // Ворог викликає це, коли входить у ChaseState
    public void StartChase()
    {
        activeChasers++;
    }

    // Ворог викликає це, коли виходить із ChaseState (наприклад, у PatrolState)
    public void StopChase()
    {
        activeChasers = Mathf.Max(0, activeChasers - 1);
    }
}
