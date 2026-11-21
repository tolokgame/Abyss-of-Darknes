using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    [Header("Керування")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    [Header("Налаштування променя")]
     private float range = 40f;
     private float spotAngle = 90f;
     private float intensity = 500f;

    private Light flashlight;

    private void Awake()
    {
        flashlight = GetComponent<Light>();

        // Примусово робимо світло спрямованим (конусом)
        flashlight.type = LightType.Spot;
        flashlight.range = range;
        flashlight.spotAngle = spotAngle;
        flashlight.intensity = intensity;

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






/*using UnityEngine;

[RequireComponent(typeof(Light))]
public class Flashlight : MonoBehaviour
{
    [Header("Керування")]
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    private Light flashlight;

    private void Awake()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = false; // початковий стан - вимкнений
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
}
*/