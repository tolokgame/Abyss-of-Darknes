using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public void OpenTheDoor()
    {
        Debug.Log("Door is opened mafaka");

        GetComponent<Animator>().enabled = true;
        Destroy(gameObject, 2f);
    }
}
