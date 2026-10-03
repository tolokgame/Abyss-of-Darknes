using UnityEngine;
using System.Collections;

public class ResonanceZone : MonoBehaviour
{
    [Header("Zone")]
    [SerializeField] private float radius = 6f;
    [SerializeField] private bool singleUse = false;
    [SerializeField] private float singleUseDuration = 3f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeSpeed = 3f;

    private Transform player;
    private bool playerInside;
    private bool hasBeenUsed;

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f;
            audioSource.Stop();
        }
    }

    private void Update()
    {
        if (player == null || audioSource == null)
            return;

        // В одноразовом режиме громкостью управляет корутина.
        if (singleUse)
            return;

        // Твой исходный код бесконечного режима — без изменений.
        float distance = Vector3.Distance(transform.position, player.position);

        float normalizedDistance = Mathf.Clamp01(distance / radius);

        float targetVolume = 1f - normalizedDistance;

        if (playerInside)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();

            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume,
                targetVolume,
                fadeSpeed * Time.deltaTime
            );
        }
        else
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume,
                0f,
                fadeSpeed * Time.deltaTime
            );

            if (audioSource.volume <= 0.01f)
                audioSource.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (singleUse && hasBeenUsed)
            return;

        player = other.transform;
        playerInside = true;

        if (singleUse)
        {
            hasBeenUsed = true;
            StartCoroutine(SingleUseRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (player != other.transform)
            return;

        playerInside = false;
        player = null;
    }

    private IEnumerator SingleUseRoutine()
    {
        if (audioSource == null)
            yield break;

        audioSource.volume = 1f;
        audioSource.Play();

        // Звук играет заданное количество секунд.
        yield return new WaitForSeconds(singleUseDuration);

        // Затем плавно затихает.
        while (audioSource.volume > 0.01f)
        {
            audioSource.volume = Mathf.MoveTowards(
                audioSource.volume,
                0f,
                fadeSpeed * Time.deltaTime
            );

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = 0f;

        playerInside = false;
        player = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}