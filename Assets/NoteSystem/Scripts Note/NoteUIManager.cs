using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    public static NoteUIManager Instance;

    [Header("UI Элементы")]
    public GameObject noteUIPanel;
    public TextMeshProUGUI noteTextDisplay;
    public GameObject interactionPrompt;
    public GameObject bottomPromptText;
    public GameObject inventoryPanel; // Ссылка на панель инвентаря

    [Header("Отключение движения при чтении")]
    public MonoBehaviour[] scriptsToDisable;

    [HideInInspector]
    public bool isReading = false;
    private NoteItem currentNoteItem;
    private bool isFromInventory = false;

    private void Awake()
    {
        Instance = this;
    }

    public void TogglePrompt(bool show)
    {
        if (!isReading && interactionPrompt != null)
        {
            interactionPrompt.SetActive(show);
        }
    }

    public void OpenNote(NoteItem note)
    {
        isReading = true;
        isFromInventory = false;
        currentNoteItem = note;

        if (noteTextDisplay != null) noteTextDisplay.text = note.noteContent;
        if (noteUIPanel != null) noteUIPanel.SetActive(true);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (bottomPromptText != null) bottomPromptText.SetActive(true);

        // Прячем инвентарь во время чтения
        if (inventoryPanel != null) inventoryPanel.SetActive(false);

        SetPlayerControl(false);
    }

    public void OpenNoteFromInventory(string content)
    {
        isReading = true;
        isFromInventory = true;
        currentNoteItem = null;

        if (noteTextDisplay != null) noteTextDisplay.text = content;
        if (noteUIPanel != null) noteUIPanel.SetActive(true);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (bottomPromptText != null) bottomPromptText.SetActive(true);

        // Прячем инвентарь во время чтения
        if (inventoryPanel != null) inventoryPanel.SetActive(false);

        SetPlayerControl(false);
    }

    public void CloseNote()
    {
        isReading = false;
        if (noteUIPanel != null) noteUIPanel.SetActive(false);

        // Возвращаем инвентарь на экран
        if (inventoryPanel != null) inventoryPanel.SetActive(true);

        currentNoteItem = null;

        SetPlayerControl(true);
    }

    public void TakeNote()
    {
        if (currentNoteItem != null && !isFromInventory)
        {
            bool added = InventoryManager.Instance.AddNoteToInventory(currentNoteItem.noteTitle, currentNoteItem.noteContent);
            if (added)
            {
                Destroy(currentNoteItem.gameObject);
            }
        }
        CloseNote();
    }

    private void SetPlayerControl(bool isEnabled)
    {
        Cursor.lockState = isEnabled ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isEnabled;

        if (scriptsToDisable != null)
        {
            foreach (var script in scriptsToDisable)
            {
                if (script != null) script.enabled = isEnabled;
            }
        }
    }

    private void Update()
    {
        if (isReading)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseNote();
            }
            if (Input.GetKeyDown(KeyCode.E) && !isFromInventory)
            {
                TakeNote();
            }
        }
    }
}