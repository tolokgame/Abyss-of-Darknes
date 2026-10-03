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

    [Header("Префаб и Игрок для выброса")]
    public GameObject notePrefab;
    public Transform playerTransform;

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
                    slot.slotBackground.color = new Color(0.2f, 0.8f, 0.2f, 0.6f);

                return true;
            }
        }
        return false;
    }

    public void UseSlot(int index)
    {
        if (index >= 0 && index < slots.Count && slots[index].isFull)
        {
            NoteUIManager.Instance.OpenNoteFromInventory(index, slots[index].noteTitle, slots[index].noteContent);
        }
    }

    public void DropNote(int slotIndex)
    {
        if (slotIndex >= 0 && slotIndex < slots.Count && slots[slotIndex].isFull)
        {
            if (notePrefab != null)
            {
                Vector3 spawnPos;
                Quaternion spawnRot;

                if (playerTransform != null)
                {
                    spawnPos = playerTransform.position + playerTransform.forward * 1.2f + Vector3.up * 0.2f;
                    spawnRot = playerTransform.rotation;
                }
                else
                {
                    Camera mainCam = Camera.main;
                    spawnPos = (mainCam != null) ? mainCam.transform.position + mainCam.transform.forward * 1.2f : transform.position;
                    spawnRot = (mainCam != null) ? mainCam.transform.rotation : Quaternion.identity;
                }

                GameObject droppedObj = Instantiate(notePrefab, spawnPos, spawnRot);
                NoteItem item = droppedObj.GetComponent<NoteItem>();
                if (item != null)
                {
                    item.noteTitle = slots[slotIndex].noteTitle;
                    item.noteContent = slots[slotIndex].noteContent;
                }
            }

            slots[slotIndex].isFull = false;
            slots[slotIndex].noteTitle = "";
            slots[slotIndex].noteContent = "";

            if (slots[slotIndex].slotTitleText != null)
                slots[slotIndex].slotTitleText.text = "Пусто";

            if (slots[slotIndex].slotBackground != null)
                slots[slotIndex].slotBackground.color = new Color(0.15f, 0.15f, 0.15f, 0.6f);
        }
    }
}