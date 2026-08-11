using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image blinkImage;
    [SerializeField] private Slider blinkSlider;

    [Header("Blink Settings")]
    [SerializeField] private float blinkInterval = 10f;
    [SerializeField] private float blinkDuration = 0.15f;
    [SerializeField] private KeyCode blinkKey = KeyCode.G;

    [Header("Hold Eyes Closed")]
    [SerializeField] private KeyCode holdBlinkKey = KeyCode.H;

    public bool IsBlinking { get; private set; }
    public bool AreEyesClosed { get; private set; }

    private float timer;

    private void Start()
    {
        timer = blinkInterval;

        SetAlpha(0f);

        blinkSlider.maxValue = blinkInterval;
        blinkSlider.value = timer;
    }

    private void Update()
    {
        // Тримаємо очі закритими
        if (Input.GetKey(holdBlinkKey))
        {
            AreEyesClosed = true;
            IsBlinking = false;
            SetAlpha(1f);

            return;
        }

        // Відкриваємо очі після відпускання кнопки
        if (AreEyesClosed)
        {
            AreEyesClosed = false;
            SetAlpha(0f);
        }

        timer -= Time.deltaTime;
        blinkSlider.value = timer;

        // Ручний blink
        if (Input.GetKeyDown(blinkKey))
        {
            timer = blinkInterval;
            StartCoroutine(Blink());
        }

        // Автоматичний blink
        if (timer <= 0)
        {
            timer = blinkInterval;
            StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        if (IsBlinking || AreEyesClosed)
            yield break;

        IsBlinking = true;

        float t = 0;

        // Закриваємо очі
        while (t < blinkDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(0, 1, t / blinkDuration));

            yield return null;
        }

        SetAlpha(1f);

        // Очі повністю закриті
        yield return new WaitForSeconds(0.05f);

        // Відкриваємо очі
        t = 0;

        while (t < blinkDuration)
        {
            t += Time.deltaTime;
            SetAlpha(Mathf.Lerp(1, 0, t / blinkDuration));

            yield return null;
        }

        SetAlpha(0f);

        IsBlinking = false;
    }

    private void SetAlpha(float alpha)
    {
        Color color = blinkImage.color;
        color.a = alpha;
        blinkImage.color = color;
    }
}



/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BlinkController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image blinkImage;
    [SerializeField] private Slider blinkSlider;

    [Header("Blink Settings")]
    [SerializeField] private float blinkInterval = 10f;
    [SerializeField] private float blinkDuration = 0.15f;
    [SerializeField] private KeyCode blinkKey = KeyCode.G;

    public bool IsBlinking { get; private set; }

    private float timer;

    private void Start()
    {
        timer = blinkInterval;

        Color color = blinkImage.color;
        color.a = 0;
        blinkImage.color = color;

        blinkSlider.maxValue = blinkInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        blinkSlider.value = timer;

        if (Input.GetKeyDown(blinkKey))
        {
            timer = blinkInterval;
            StartCoroutine(Blink());
        }

        if (timer <= 0)
        {
            timer = blinkInterval;
            StartCoroutine(Blink());
        }
    }

    private IEnumerator Blink()
    {
        if (IsBlinking)
            yield break;

        IsBlinking = true;

        
        float t = 0;

        while (t < blinkDuration)
        {
            t += Time.deltaTime;

            Color color = blinkImage.color;
            color.a = Mathf.Lerp(0, 1, t / blinkDuration);
            blinkImage.color = color;

            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        
        t = 0;

        while (t < blinkDuration)
        {
            t += Time.deltaTime;

            Color color = blinkImage.color;
            color.a = Mathf.Lerp(1, 0, t / blinkDuration);
            blinkImage.color = color;

            yield return null;
        }

        Color finalColor = blinkImage.color;
        finalColor.a = 0;
        blinkImage.color = finalColor;

        IsBlinking = false;
    }
}*/