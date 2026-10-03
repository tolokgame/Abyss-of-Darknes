using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Настройки луча")]
    public float interactDistance = 3.5f;
    public LayerMask interactLayer;

    private NoteItem targetNote;

    private void Update()
    {
        if (NoteUIManager.Instance != null && NoteUIManager.Instance.isReading)
            return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            NoteItem note = hit.collider.GetComponent<NoteItem>();

            if (note != null)
            {
                if (targetNote != note)
                {
                    ResetTarget();
                    targetNote = note;
                    targetNote.SetHighlight(true);
                    NoteUIManager.Instance.TogglePrompt(true);
                }

                if (Input.GetKeyDown(KeyCode.X))
                {
                    NoteUIManager.Instance.OpenNote(targetNote);
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
            targetNote = null;
            if (NoteUIManager.Instance != null)
            {
                NoteUIManager.Instance.TogglePrompt(false);
            }
        }
    }
}