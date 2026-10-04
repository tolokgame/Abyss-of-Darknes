using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    [Header("Керування")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    private Light flashlight;

    private void Awake()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = false; 
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}