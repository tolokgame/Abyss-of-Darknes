using System.Collections.Generic;
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
