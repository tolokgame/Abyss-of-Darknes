using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float MaxStamina = 100f;
    [SerializeField] private float StaminaDrain = 10f;
    [SerializeField] private float StaminaRegen = 15f;

    [Header("Movement")]
    [SerializeField] private float WalkSpeed = 5f;
    [SerializeField] private float RunSpeed = 8f;

    [Header("UI")]
    [SerializeField] private Slider SliderStamina;

    [Header("Timer")]
    [SerializeField] private float StaminaRegenTimer = 2f;

    [Header("RunKey")]
    [SerializeField] private KeyCode RunKey = KeyCode.LeftShift;

    private float regenTimer = 0f;
    private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();

        stamina = MaxStamina;

        if (SliderStamina != null)
        {
            SliderStamina.maxValue = MaxStamina;
            SliderStamina.value = stamina;
        }
    }

    private void Update()
    {
        bool isRunning = Input.GetKey(RunKey) && stamina > 0f;

        // Біг
        if (isRunning)
        {
            stamina -= StaminaDrain * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, MaxStamina);

            regenTimer = 0f;

            if (playerController != null)
                playerController.moveSpeed = RunSpeed;
        }
        else
        {
            // Повертаємо швидкість ходьби
            if (playerController != null)
                playerController.moveSpeed = WalkSpeed;

            // Відновлення stamina після паузи
            regenTimer += Time.deltaTime;

            if (regenTimer >= StaminaRegenTimer)
            {
                stamina += StaminaRegen * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0f, MaxStamina);
            }
        }

        if (SliderStamina != null)
            SliderStamina.value = stamina;
    }
}






/*using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina")]
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float MaxStamina = 100f;
    [SerializeField] private float StaminaDrain = 10f;
    [SerializeField] private float StaminaRegen = 15f;
    [SerializeField] private float StaminaRegenTimer = 0f;

    [Header("UI")]
    [SerializeField] private Slider SliderStamina;
   
    [Header("Timer")]
    [SerializeField] private float OutOfBreathTimer = 5f;

    [Header("RunKey")]
    [SerializeField] private KeyCode RunKey = KeyCode.LeftShift;

    private void Update()
    {
        if (Input.GetKey(RunKey))
        {
            stamina -= StaminaDrain * Time.deltaTime;
            stamina = Mathf.Clamp(stamina, 0f, MaxStamina);
        }

        if (!Input.GetKey(RunKey))
        {
            StaminaRegenTimer += Time.deltaTime;

            if (StaminaRegenTimer >= 5f)
            {
                stamina += StaminaRegen * Time.deltaTime;
                stamina = Mathf.Clamp(stamina, 0f, MaxStamina);
            }
        }
        else
        {
            StaminaRegenTimer = 0f;
        }

        SliderStamina.value = stamina;
    }
}*/