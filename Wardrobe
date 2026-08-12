using UnityEngine;

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
