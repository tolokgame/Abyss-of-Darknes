using UnityEngine;

public class DestroyOnItemCount : MonoBehaviour
{
    [Header("Налаштування умови")]
    [SerializeField] private PlayerInventory playerInventory; // Посилання на інвентар гравця
    [SerializeField] private ItemData requiredItem;           // Перетягніть сюди ItemData вашої кульки
    [SerializeField] private int requiredAmount = 50;         // Потрібна кількість (50)

    [Header("Додатково")]
    [SerializeField] private bool removeItemsAfterDestroy = false; // Чи забирати кульки з інвентарю?

    private void Start()
    {
        // Якщо забули вказати інвентар вручну, шукаємо його на сцені автоматично
        if (playerInventory == null)
        {
            playerInventory = FindFirstObjectByType<PlayerInventory>();
        }

        // Підписуємось на оновлення інвентарю
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged += CheckCondition;
            CheckCondition(); // Перевіряємо одразу на старті
        }
    }

    private void OnDestroy()
    {
        // Обов'язково відписуємось від події при знищенні об'єкта
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= CheckCondition;
        }
    }

    private void CheckCondition()
    {
        if (playerInventory == null || requiredItem == null) return;

        int currentAmount = playerInventory.GetItemCount(requiredItem);

        if (currentAmount >= requiredAmount)
        {
            // Якщо потрібно забрати кульки з інвентарю при знищенні об'єкта
            if (removeItemsAfterDestroy)
            {
                for (int i = 0; i < requiredAmount; i++)
                {
                    playerInventory.RemoveItem(requiredItem);
                }
            }

            Debug.Log($"[Condition] Зібрано {currentAmount}/{requiredAmount} {requiredItem.itemName}. Об'єкт {gameObject.name} знищено!");
            Destroy(gameObject);
        }
    }
}
