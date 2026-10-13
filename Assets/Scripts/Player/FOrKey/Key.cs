using UnityEngine;

public class Key : MonoBehaviour
{

    [SerializeField] private OpenDoor doorToOpen;
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if(inventory != null )
        {
             UseKey();
        }
    }

    private void UseKey()
    {
        if(doorToOpen = null)
        {
            doorToOpen.OpenTheDoor();
        }
        
        Destroy(gameObject);
    }
}
