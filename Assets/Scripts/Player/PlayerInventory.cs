using System; // Додайте цей простір імен для подій (Action)
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> items = new List<ItemData>();

    // Подія, яка спрацьовує при будь-якій зміні інвентарю
    public event Action OnInventoryChanged;

    public bool AddItem(ItemData item)
    {
        if (item == null) return false;

        items.Add(item);
        Debug.Log($"[Inventory] Додано: {item.itemName} (ID: {item.itemID}). Всього предметів: {items.Count}");

        OnInventoryChanged?.Invoke(); // Повідомляємо інші скрипти, що предмет додано
        return true;
    }

    public bool RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"[Inventory] Видалено: {item.itemName}. Залишилось предметів: {items.Count}");

            OnInventoryChanged?.Invoke(); // Повідомляємо про видалення
            return true;
        }

        Debug.Log($"[Inventory] Предмета {item?.itemName} немає в інвентарі.");
        return false;
    }

    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }

    // НОВИЙ МЕТОД: рахує кількість конкретного предмета (наприклад, кульок)
    public int GetItemCount(ItemData targetItem)
    {
        int count = 0;
        foreach (ItemData item in items)
        {
            if (item == targetItem)
            {
                count++;
            }
        }
        return count;
    }
}

















/*using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<ItemData> items = new List<ItemData>(); // Наш список предметів

    // Додати предмет
    public bool AddItem(ItemData item)
    {
        if (item == null) return false;

        items.Add(item);
        Debug.Log($"[Inventory] Додано: {item.itemName} (ID: {item.itemID}). Всього предметів: {items.Count}");
        return true;
    }

    // Видалити предмет
    public bool RemoveItem(ItemData item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log($"[Inventory] Видалено: {item.itemName}. Залишилось предметів: {items.Count}");
            return true;
        }

        Debug.Log($"[Inventory] Предмета {item?.itemName} немає в інвентарі.");
        return false;
    }

    // Перевірити, чи є такий предмет
    public bool HasItem(ItemData item)
    {
        return items.Contains(item);
    }
}
*/