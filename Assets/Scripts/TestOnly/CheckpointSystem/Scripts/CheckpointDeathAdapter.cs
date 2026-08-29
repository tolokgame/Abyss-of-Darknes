using UnityEngine;

/// <summary>
/// Optional bridge for an existing death system.
/// Add this component to the player and call NotifyDeath() from your death code.
/// </summary>
public class CheckpointDeathAdapter : MonoBehaviour
{
    public void NotifyDeath()
    {
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.NotifyPlayerDeath(gameObject);
    }

    public void NotifyDeath(GameObject deadPlayer)
    {
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.NotifyPlayerDeath(deadPlayer);
    }

    public void NotifyRevived()
    {
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.NotifyPlayerRevived(transform);
    }
}
