using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator doorAnimator;

    void Start()
    {
        doorAnimator = GetComponent<Animator>();
    }

    public void OpenTheDoor()
    {
        if (doorAnimator != null)
        {
            doorAnimator.Play("Door_Open");
            Debug.Log("Двері відчиняються через функцію OpenTheDoor!");
        }
        else
        {
            Debug.LogError("На дверях немає компонента Animator!");
        }
    }
}
