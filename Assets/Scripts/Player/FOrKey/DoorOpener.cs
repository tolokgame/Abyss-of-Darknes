using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private OpenDoor doorToOpen;
    [SerializeField] private ItemData keyRequired; // Сюди в інспекторі треба перетягнути ScriptableObject твого ключа

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            // ПЕРЕВІРКА: чи є у гравця потрібний ключ?
            if (inventory.HasItem(keyRequired))
            {
                UseKey(inventory);
            }
            else
            {
                Debug.Log("Двері замкнені. Вам потрібен ключ: " + (keyRequired != null ? keyRequired.itemName : "Не задано"));
            }
        }
    }

    private void UseKey(PlayerInventory inventory)
    {
        if (doorToOpen != null)
        {
            doorToOpen.OpenTheDoor();

            // Забираємо ключ з інвентаря після використання (якщо це потрібно)
            inventory.RemoveItem(keyRequired);

            // Знищуємо тригер дверей, щоб вони не відкривалися повторно
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("Жах скасовується, але ти забув перетягнути двері в поле Door To Open!");
        }
    }
}




/*using UnityEngine;

public class DoorOpener : MonoBehaviour
{
    [SerializeField] private OpenDoor doorToOpen;

    private void OnTriggerEnter(Collider other)
    {

        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            UseKey();
        }

   
    }

    private void UseKey()
    {
        if (doorToOpen != null)
        {
            doorToOpen.OpenTheDoor(); 
        }
        else
        {
            Debug.LogError("Жах скасовується, але ти забув перетягнути двері в поле Door To Open!");
        }


        Destroy(gameObject);
    }
}
*/