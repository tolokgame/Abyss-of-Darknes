using UnityEngine;
using TMPro;

public class NoteUIManager : MonoBehaviour
{
    public static NoteUIManager Instance;

    [Header("UI Элементы")]
    public GameObject noteUIPanel;
    public TextMeshProUGUI noteTextDisplay;
    public GameObject interactionPrompt;

    [HideInInspector]
    public bool isReading = false;
    private GameObject currentNoteObject;

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

    public void OpenNote(string content, GameObject noteObj)
    {
        isReading = true;
        currentNoteObject = noteObj;

        if (noteTextDisplay != null) noteTextDisplay.text = content;
        if (noteUIPanel != null) noteUIPanel.SetActive(true);
        if (interactionPrompt != null) interactionPrompt.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseNote()
    {
        isReading = false;
        if (noteUIPanel != null) noteUIPanel.SetActive(false);
        currentNoteObject = null;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void TakeNote()
    {
        if (currentNoteObject != null)
        {
            Destroy(currentNoteObject);
        }
        CloseNote();
    }

    private void Update()
    {
        if (isReading)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                CloseNote();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeNote();
            }
        }
    }
}