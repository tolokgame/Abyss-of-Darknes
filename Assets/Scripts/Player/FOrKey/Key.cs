using UnityEngine;

public class Key : MonoBehaviour
{
    // —юди в ≥нспектор≥ перет€гуЇмо об'Їкт дверей
    [SerializeField] private OpenDoor doorToOpen;

    private void OnTriggerEnter(Collider other)
    {
        // ѕерев≥р€Їмо, чи у об'Їкта, €кий наступив на ключ, Ї скрипт PlayerInventory
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
            doorToOpen.OpenTheDoor(); // ¬икликаЇмо функц≥ю в≥дкритт€ дверей
        }
        else
        {
            Debug.LogError("∆ах скасовуЇтьс€, але ти забув перет€гнути двер≥ в поле Door To Open!");
        }

        // «нищуЇмо ключ (в≥н зникаЇ з карти)
        Destroy(gameObject);
    }
}
