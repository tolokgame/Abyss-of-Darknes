using UnityEngine;

/// <summary>
/// Stores the last activated checkpoint and respawns the player there.
/// Uses Unity's built-in Input Manager for R by default.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject playerPrefab;

    [Header("Input")]
    [SerializeField] private KeyCode respawnKey = KeyCode.R;
    [Tooltip("If enabled, R works only after NotifyPlayerDeath() has been called.")]
    [SerializeField] private bool requireDeathSignal = true;

    [Header("Save")]
    [SerializeField] private bool persistBetweenScenes = true;
    [SerializeField] private bool saveImmediately = true;
    [SerializeField] private string saveKey = "Abyss_Checkpoint";

    [Header("Respawn")]
    [SerializeField] private float verticalOffset = 0.15f;

    private Vector3 checkpointPosition;
    private Quaternion checkpointRotation = Quaternion.identity;
    private bool hasCheckpoint;
    private bool playerIsDead;
    private GameObject playerPrefabOverride;

    private const string HasSuffix = ".Has";
    private const string PX = ".PX";
    private const string PY = ".PY";
    private const string PZ = ".PZ";
    private const string RY = ".RY";

    public bool HasCheckpoint => hasCheckpoint;
    public bool PlayerIsDead => playerIsDead;

    private string Key(string suffix) => saveKey + suffix;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (persistBetweenScenes)
            DontDestroyOnLoad(gameObject);

        LoadCheckpoint();
    }

    private void Update()
    {
        if (Input.GetKeyDown(respawnKey) && (!requireDeathSignal || playerIsDead))
            RespawnPlayer();
    }

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
    }

    public void SetPlayerPrefab(GameObject prefab)
    {
        playerPrefabOverride = prefab;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint == null)
            return;

        checkpointPosition = checkpoint.position;
        checkpointRotation = checkpoint.rotation;
        hasCheckpoint = true;

        SaveCheckpoint();
    }

    /// <summary>
    /// Call this from the existing death system immediately before it destroys the player.
    /// </summary>
    public void NotifyPlayerDeath(GameObject deadPlayer)
    {
        if (deadPlayer != null)
            player = deadPlayer.transform;

        playerIsDead = true;
    }

    /// <summary>
    /// Optional call for a death system that does not destroy the player.
    /// </summary>
    public void NotifyPlayerRevived(Transform revivedPlayer)
    {
        if (revivedPlayer != null)
            player = revivedPlayer;

        playerIsDead = false;
    }

    public void RespawnPlayer()
    {
        if (!hasCheckpoint)
        {
            Debug.LogWarning("CheckpointManager: no checkpoint has been activated yet.");
            return;
        }

        Transform targetPlayer = player;

        if (targetPlayer == null)
        {
            GameObject prefab = playerPrefabOverride != null ? playerPrefabOverride : playerPrefab;

            if (prefab == null)
            {
                Debug.LogWarning("CheckpointManager: Player is missing and no Player Prefab is assigned.");
                return;
            }

            GameObject spawned = Instantiate(
                prefab,
                checkpointPosition + Vector3.up * verticalOffset,
                checkpointRotation);

            player = spawned.transform;
            playerIsDead = false;
            return;
        }

        CharacterController controller = targetPlayer.GetComponent<CharacterController>();
        Rigidbody rb = targetPlayer.GetComponent<Rigidbody>();

        if (controller != null)
            controller.enabled = false;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        targetPlayer.SetPositionAndRotation(
            checkpointPosition + Vector3.up * verticalOffset,
            checkpointRotation);

        if (rb != null)
            rb.isKinematic = false;

        if (controller != null)
            controller.enabled = true;

        playerIsDead = false;
    }

    private void SaveCheckpoint()
    {
        PlayerPrefs.SetInt(Key(HasSuffix), 1);
        PlayerPrefs.SetFloat(Key(PX), checkpointPosition.x);
        PlayerPrefs.SetFloat(Key(PY), checkpointPosition.y);
        PlayerPrefs.SetFloat(Key(PZ), checkpointPosition.z);
        PlayerPrefs.SetFloat(Key(RY), checkpointRotation.eulerAngles.y);

        if (saveImmediately)
            PlayerPrefs.Save();
    }

    private void LoadCheckpoint()
    {
        if (PlayerPrefs.GetInt(Key(HasSuffix), 0) == 0)
        {
            hasCheckpoint = false;
            return;
        }

        checkpointPosition = new Vector3(
            PlayerPrefs.GetFloat(Key(PX)),
            PlayerPrefs.GetFloat(Key(PY)),
            PlayerPrefs.GetFloat(Key(PZ)));

        checkpointRotation = Quaternion.Euler(
            0f,
            PlayerPrefs.GetFloat(Key(RY)),
            0f);

        hasCheckpoint = true;
    }

    public void ClearCheckpointSave()
    {
        hasCheckpoint = false;
        playerIsDead = false;

        PlayerPrefs.DeleteKey(Key(HasSuffix));
        PlayerPrefs.DeleteKey(Key(PX));
        PlayerPrefs.DeleteKey(Key(PY));
        PlayerPrefs.DeleteKey(Key(PZ));
        PlayerPrefs.DeleteKey(Key(RY));
        PlayerPrefs.Save();
    }
}
