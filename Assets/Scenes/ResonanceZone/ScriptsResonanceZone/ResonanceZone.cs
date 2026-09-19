using UnityEngine;

public class ResonanceZone : MonoBehaviour
{
    [Header("Zone")]
    [SerializeField] private float radius = 6f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeSpeed = 3f;

    private Transform player;
    private bool playerInside;

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

        player = other.transform;
        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
        player = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}