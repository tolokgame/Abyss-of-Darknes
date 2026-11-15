using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(MeshRenderer))]
public class NoteItem : MonoBehaviour
{
    [Header("Note Content")]
    public string noteTitle = "Note";
    [TextArea(6, 12)]
    public string noteContent;

    [Header("Highlight")]
    public Material highlightMaterial;

    private Renderer meshRenderer;
    private Material originalMaterial;
    private bool isHighlighted;

    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        originalMaterial = meshRenderer.sharedMaterial;

        if (highlightMaterial == null)
        {
            throw new System.InvalidOperationException($"Note '{name}' requires a highlight material.");
        }
    }

    public void SetHighlight(bool isHighlighted)
    {
        this.isHighlighted = isHighlighted;
        meshRenderer.sharedMaterial = isHighlighted ? highlightMaterial : originalMaterial;
    }

    private void OnDisable()
    {
        if (isHighlighted)
        {
            SetHighlight(false);
        }
    }
}
