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
}