using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [System.Serializable]
    public class InventorySlot
    {
        public Image slotBackground;
        public TextMeshProUGUI slotNumberText;
        public TextMeshProUGUI slotTitleText;
        [HideInInspector] public string noteTitle;
        [HideInInspector] public string noteContent;
        [HideInInspector] public bool isFull = false;
    }

    public List<InventorySlot> slots = new List<InventorySlot>(3);

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (NoteUIManager.Instance != null && NoteUIManager.Instance.isReading)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) UseSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseSlot(2);
    }

    public bool AddNoteToInventory(string title, string content)
    {
        foreach (var slot in slots)
        {
            if (!slot.isFull)
            {
                slot.isFull = true;
                slot.noteTitle = title;
                slot.noteContent = content;

                if (slot.slotTitleText != null)
                    slot.slotTitleText.text = title;

                if (slot.slotBackground != null)
                    slot.slotBackground.color = new Color(0.2f, 0.8f, 0.2f, 0.6f); // Зелёная подсветка слота

                return true;
            }
        }
        return false;
    }

    public void UseSlot(int index)
    {
        if (index >= 0 && index < slots.Count && slots[index].isFull)
        {
            NoteUIManager.Instance.OpenNoteFromInventory(slots[index].noteContent);
        }
    }
}