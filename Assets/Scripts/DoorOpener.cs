using UnityEngine;

public class DoorOpener : MonoBehaviour
{
	public GameObject doorObject;
	public GameObject Key;
	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == Key)
		{
			doorObject.GetComponent<DoorScript.Door>().OpenDoor();

			Destroy(other.gameObject);
		}
	}
}
