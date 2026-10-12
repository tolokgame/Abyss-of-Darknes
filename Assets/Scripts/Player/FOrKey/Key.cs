using UnityEngine;

public class Key : MonoBehaviour
{
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
        
    }
}
