using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class NoteUIManager : MonoBehaviour
{
    public static NoteUIManager Instance { get; private set; }

    // Keep the close key from reaching gameplay interactions later in the same frame.
    public static bool BlocksGameplayInput => Instance != null &&
        (Instance.IsReading || Instance.lastClosedFrame == Time.frameCount);

    [Header("UI Elements")]
    public GameObject noteUIPanel;
    public TextMeshProUGUI noteTitleDisplay;
    public TextMeshProUGUI noteTextDisplay;
    public GameObject interactionPrompt;
    public GameObject bottomPromptText;
    public GameObject crosshair;

    [Header("Player Controls")]
    public MonoBehaviour[] scriptsToDisable = new MonoBehaviour[0];

    public bool IsReading { get; private set; }

    private bool[] previousScriptStates;
    private bool crosshairWasActive;
    private int lastClosedFrame = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            throw new System.InvalidOperationException("Only one NoteUIManager may be active at a time.");
        }

        if (noteUIPanel == null || noteTitleDisplay == null || noteTextDisplay == null ||
            interactionPrompt == null || bottomPromptText == null || crosshair == null)
        {
            throw new System.InvalidOperationException($"Note UI '{name}' has missing UI references.");
        }

        previousScriptStates = new bool[scriptsToDisable.Length];
        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            if (scriptsToDisable[i] == null || scriptsToDisable[i] == this || scriptsToDisable[i] is PlayerInteraction)
            {
                throw new System.InvalidOperationException($"Note UI '{name}' has an invalid player control reference.");
            }
        }

        Instance = this;
        noteUIPanel.SetActive(false);
        interactionPrompt.SetActive(false);
    }

    public void TogglePrompt(bool show)
    {
        interactionPrompt.SetActive(show && !IsReading);
    }

    public void OpenNote(NoteItem note)
    {
        if (IsReading)
        {
            throw new System.InvalidOperationException("Close the current note before opening another one.");
        }

        IsReading = true;
        noteTitleDisplay.text = note.noteTitle;
        noteTextDisplay.text = note.noteContent;
        noteUIPanel.SetActive(true);
        interactionPrompt.SetActive(false);
        bottomPromptText.SetActive(true);
        crosshairWasActive = crosshair.activeSelf;
        crosshair.SetActive(false);

        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            previousScriptStates[i] = scriptsToDisable[i].enabled;
            scriptsToDisable[i].enabled = false;
        }
    }

    public void CloseNote()
    {
        if (!IsReading)
        {
            return;
        }

        IsReading = false;
        lastClosedFrame = Time.frameCount;
        noteUIPanel.SetActive(false);
        bottomPromptText.SetActive(false);
        crosshair.SetActive(crosshairWasActive);

        for (int i = 0; i < scriptsToDisable.Length; i++)
        {
            // Scene unload can destroy player components before the UI.
            if (scriptsToDisable[i] != null)
            {
                scriptsToDisable[i].enabled = previousScriptStates[i];
            }
        }
    }

    private void OnDisable()
    {
        if (IsReading)
        {
            CloseNote();
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
