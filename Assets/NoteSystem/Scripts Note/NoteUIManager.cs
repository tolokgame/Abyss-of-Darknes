using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    public static NoteUIManager Instance;

    [Header("UI Елементи")]
    public GameObject noteUIPanel;
    public TextMeshProUGUI noteTextDisplay;
    public GameObject interactionPrompt;
    public GameObject bottomPromptText;

    [Header("Вимкнення руху при читанні")]
    public MonoBehaviour[] scriptsToDisable;

    [HideInInspector]
    public bool isReading = false;

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

        if (noteTextDisplay != null) noteTextDisplay.text = note.noteContent;
        if (noteUIPanel != null) noteUIPanel.SetActive(true);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);
        if (bottomPromptText != null) bottomPromptText.SetActive(true);

        SetPlayerControl(false);
    }

    public void CloseNote()
    {
        isReading = false;
        if (noteUIPanel != null) noteUIPanel.SetActive(false);

        SetPlayerControl(true);
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
            // Тепер будь-яка з цих клавіш (X, Escape або E) просто закриває записку
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                CloseNote();
            }
        }
    }
}