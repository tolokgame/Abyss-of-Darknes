using UnityEngine;

/// <summary>
/// Put this component on a checkpoint prefab. The prefab must have a trigger collider.
/// </summary>
[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool oneTimeActivation = true;

    [Header("Effects")]
    [SerializeField] private CheckpointVisual visual;

    private bool activated;

    private void Awake()
    {
        Collider trigger = GetComponent<Collider>();
        trigger.isTrigger = true;

        if (respawnPoint == null)
            respawnPoint = transform;

        if (visual == null)
            visual = GetComponentInChildren<CheckpointVisual>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (oneTimeActivation && activated)
            return;

        Transform playerTransform = FindPlayerTransform(other);
        if (playerTransform == null)
            return;

        Activate(playerTransform);
    }

    private Transform FindPlayerTransform(Collider other)
    {
        if (other.CompareTag(playerTag))
            return other.transform;

        Transform root = other.transform.root;
        if (root != null && root.CompareTag(playerTag))
            return root;

        return null;
    }

    private void Activate(Transform playerTransform)
    {
        if (CheckpointManager.Instance == null)
        {
            Debug.LogWarning("Checkpoint: CheckpointManager was not found in the scene.");
            return;
        }

        activated = true;

        CheckpointManager.Instance.SetPlayer(playerTransform);
        CheckpointManager.Instance.SetCheckpoint(respawnPoint);

        if (visual != null)
            visual.Activate();
    }

    public void ResetCheckpointVisual()
    {
        activated = false;
        visual?.Deactivate();
    }
}
