using UnityEngine;

public class NoteItem : MonoBehaviour
{
    [Header("Настройки Записки")]
    public string noteTitle = "Записка 1";
    [TextArea(6, 12)]
    public string noteContent = "Никогда не теряй надежды.\nНе знаешь, что делать? Оглянись вокруг.";

    [Header("Материалы")]
    public Material defaultMaterial;
    public Material highlightMaterial;

    private Renderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
    }

    public void SetHighlight(bool isHighlighted)
    {
        if (meshRenderer != null && highlightMaterial != null && defaultMaterial != null)
        {
            meshRenderer.material = isHighlighted ? highlightMaterial : defaultMaterial;
        }
    }
}