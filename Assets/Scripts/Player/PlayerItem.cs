using UnityEngine;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private KeyCode pickUpKey = KeyCode.E;

    private PlayerInventory inventoryInRange; // Зберігаємо посилання на інвентар, поки гравець поруч

    private void Update()
    {
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



