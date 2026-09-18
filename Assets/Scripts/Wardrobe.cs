using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Positions")]
    [SerializeField] private Transform insidePoint;
    [SerializeField] private Transform cameraPoint;
    [SerializeField] private Transform exitPoint;

    private bool isPlayerInside = false;
    private bool isPlayerInZone = false;

    private GameObject playerObject;
    private PlayerController playerController;
    private PlayerState playerState;

    private Vector3 previousPlayerPosition;
    private Quaternion previousPlayerRotation;

    private void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactKey))
        {
            if (!isPlayerInside)
                HidePlayer();
            else
                ExitWardrobe();
        }
    }

    private void HidePlayer()
    {
        if (playerObject == null)
            return;

        playerController = playerObject.GetComponent<PlayerController>();
        playerState = playerObject.GetComponent<PlayerState>();

        isPlayerInside = true;

        // Запам'ятовуємо позицію перед входом
        previousPlayerPosition = playerObject.transform.position;
        previousPlayerRotation = playerObject.transform.rotation;

        // Переміщуємо гравця всередину шафи
        if (insidePoint != null)
        {
            playerObject.transform.position = insidePoint.position;
            playerObject.transform.rotation = insidePoint.rotation;
        }
        else
        {
            playerObject.transform.position = transform.position;
        }

        // Переміщуємо камеру в потрібне місце
        if (cameraPoint != null && playerController != null)
        {
            playerController.cameraHolder.position = cameraPoint.position;
            playerController.cameraHolder.rotation = cameraPoint.rotation;
        }

        // Вмикаємо стан Hidden
        if (playerState != null)
        {
            playerState.IsHidden = true;
        }
    }

    private void ExitWardrobe()
    {
        if (playerObject == null)
            return;

        // Вимикаємо Hidden
        if (playerState != null)
        {
            playerState.IsHidden = false;
        }

        // Виводимо гравця
        if (exitPoint != null)
        {
            playerObject.transform.position = exitPoint.position;
            playerObject.transform.rotation = exitPoint.rotation;
        }
        else
        {
            playerObject.transform.position = previousPlayerPosition;
            playerObject.transform.rotation = previousPlayerRotation;
        }

        isPlayerInside = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInZone = true;
        playerObject = other.gameObject;

        playerController = playerObject.GetComponent<PlayerController>();
        playerState = playerObject.GetComponent<PlayerState>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInZone = false;

        // Якщо гравець не всередині шафи,
        // можна забути його.
        if (!isPlayerInside)
        {
            playerObject = null;
            playerController = null;
            playerState = null;
        }
    }
}




/*using UnityEngine;

public class Wardrobe : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private Transform insidePoint; 

    private bool isPlayerInside = false;
    private bool isPlayerInZone = false;
    private GameObject playerObject;
    private Vector3 previousPlayerPosition;

    void Update()
    {
        if (isPlayerInZone && Input.GetKeyDown(interactKey))
        {
            if (!isPlayerInside)
                HidePlayer();
            else
                ExitWardrobe();
        }
    }

    private void HidePlayer()
    {
        isPlayerInside = true;
        previousPlayerPosition = playerObject.transform.position;

        PlayerState playerState = playerObject.GetComponent<PlayerState>();
        Rigidbody playerRb = playerObject.GetComponent<Rigidbody>();

        
        if (playerRb != null)
        {
            playerRb.isKinematic = true;
        }

        if (insidePoint != null)
        {
            playerObject.transform.position = insidePoint.position;
        }
        else
        {
            playerObject.transform.position = transform.position;
        }

        if (playerState != null)
        {
            playerState.IsHidden = true;
        }

    }

    private void ExitWardrobe()
    {
        isPlayerInside = false;

        PlayerState playerState = playerObject.GetComponent<PlayerState>();
        Rigidbody playerRb = playerObject.GetComponent<Rigidbody>();

        playerObject.transform.position = previousPlayerPosition;

        if (playerRb != null)
        {
            playerRb.isKinematic = false;
        }

        if (playerState != null)
        {
            playerState.IsHidden = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            playerObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (!isPlayerInside) playerObject = null;
        }
    }
}
*/