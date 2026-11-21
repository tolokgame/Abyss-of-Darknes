using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [Header("Налаштування предмета")]
    [SerializeField] private ItemData itemData;
    [SerializeField] private bool autoPickUp = false; // Галочка автопідбору
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;

    [Header("Звук")]
    [SerializeField] private AudioClip pickUpSound;
    [SerializeField][Range(0f, 1f)] private float soundVolume = 1f;

    private PlayerInventory inventoryInRange;

    private void Update()
    {
        // Якщо увімкнено автопідбір або заблоковано керування, Update нічого не робить
        if (autoPickUp || NoteUIManager.BlocksGameplayInput)
        {
            return;
        }

        if (inventoryInRange != null && Input.GetKeyDown(pickUpKey))
        {
            TryPickUp(inventoryInRange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            // Якщо увімкнено Auto Pick Up — підбираємо одразу
            if (autoPickUp)
            {
                TryPickUp(playerInventory);
            }
            else
            {
                // Інакше запам'ятовуємо інвентар і чекаємо натискання клавіші
                inventoryInRange = playerInventory;
                Debug.Log($"[Item] Натисніть {pickUpKey}, щоб підібрати {itemData?.itemName}");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null && inventoryInRange == playerInventory)
        {
            inventoryInRange = null;
            Debug.Log("[Item] Гравець відійшов від предмета.");
        }
    }

    // Окремий метод для додавання предмета, програвання звуку та знищення об'єкта
    private void TryPickUp(PlayerInventory targetInventory)
    {
        bool isAdded = targetInventory.AddItem(itemData);

        if (isAdded)
        {
            if (pickUpSound != null)
            {
                AudioSource.PlayClipAtPoint(pickUpSound, transform.position, soundVolume);
            }

            Destroy(gameObject);
        }
    }
}








/*using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;

    [Header("Звук")]
    [SerializeField] private AudioClip pickUpSound; // Поле для вашого аудіофайлу
    [SerializeField][Range(0f, 1f)] private float soundVolume = 1f; // Повзунок гучності

    private PlayerInventory inventoryInRange;

    private void Update()
    {
        if (NoteUIManager.BlocksGameplayInput)
        {
            return;
        }

        if (inventoryInRange != null && Input.GetKeyDown(pickUpKey))
        {
            bool isAdded = inventoryInRange.AddItem(itemData);

            if (isAdded)
            {
                // Програємо звук на місці предмета перед його видаленням
                if (pickUpSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickUpSound, transform.position, soundVolume);
                }

                Destroy(gameObject); // Видаляємо предмет зі сцени
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            inventoryInRange = playerInventory;
            Debug.Log($"[Item] Натисніть {pickUpKey}, щоб підібрати {itemData?.itemName}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null && inventoryInRange == playerInventory)
        {
            inventoryInRange = null;
            Debug.Log("[Item] Гравець відійшов від предмета.");
        }
    }
}
*/





/*using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;

    private PlayerInventory inventoryInRange; // Зберігаємо посилання на інвентар, поки гравець поруч

    private void Update()
    {
        if (NoteUIManager.BlocksGameplayInput)
        {
            return;
        }

        Debug.Log(Input.GetKeyDown(pickUpKey));
        // Якщо гравець поруч І він натиснув клавішу 'E'
        if (inventoryInRange != null && Input.GetKeyDown(pickUpKey))
        {
            bool isAdded = inventoryInRange.AddItem(itemData);

            if (isAdded)
            {
                Destroy(gameObject); // Видаляємо предмет зі сцени
            }
        }
    }
   
    // Гравець підійшов до предмета
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if (playerInventory != null)
        {
            inventoryInRange = playerInventory; // Запам'ятовуємо інвентар гравця
            Debug.Log($"[Item] Натисніть {pickUpKey}, щоб підібрати {itemData?.itemName}");
        }
    }

    // Гравець відійшов від предмета
    private void OnTriggerExit(Collider other)
    {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        // Перевіряємо, чи це саме той гравець, що відійшов
        if (playerInventory != null && inventoryInRange == playerInventory)
        {
            inventoryInRange = null; // Забуваємо інвентар, підібрати більше не можна
            Debug.Log("[Item] Гравець відійшов від предмета.");
        }
    }
}
*/


