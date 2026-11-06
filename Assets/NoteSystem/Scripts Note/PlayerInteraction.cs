using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Ray")]
    public float interactDistance = 3.5f;
    public LayerMask interactLayer;

    private NoteItem targetNote;
    private NoteUIManager noteUIManager;

    private void Start()
    {
        noteUIManager = NoteUIManager.Instance;
        if (noteUIManager == null)
        {
            throw new System.InvalidOperationException("PlayerInteraction requires a NoteUIManager in the scene.");
        }
    }

    private void Update()
    {
        if (noteUIManager.IsReading)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                noteUIManager.CloseNote();
            }

            return;
        }

        if (!noteUIManager.isActiveAndEnabled)
        {
            ResetTarget();
            return;
        }

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Hit the nearest solid surface first so walls can block interaction.
        if (Physics.Raycast(ray, out hit, interactDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore) &&
            (interactLayer.value & (1 << hit.collider.gameObject.layer)) != 0)
        {
            NoteItem note = hit.collider.GetComponentInParent<NoteItem>();
            if (note != null && note.isActiveAndEnabled)
            {
                if (targetNote != note)
                {
                    ResetTarget();
                    targetNote = note;
                    targetNote.SetHighlight(true);
                    noteUIManager.TogglePrompt(true);
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    noteUIManager.OpenNote(targetNote);
                    ResetTarget();
                }

                return;
            }
        }

        ResetTarget();
    }

    private void ResetTarget()
    {
        if (targetNote != null)
        {
            targetNote.SetHighlight(false);
        }

        targetNote = null;
        if (noteUIManager != null)
        {
            noteUIManager.TogglePrompt(false);
        }
    }

    private void OnDisable()
    {
        ResetTarget();
        if (noteUIManager != null && noteUIManager.IsReading)
        {
            noteUIManager.CloseNote();
        }
    }
}
