using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Note Content")]
    [TextArea(5, 20)]
    public string noteText;

    public string noteTitle = "NOTE";

    [Header("Interaction")]
    public float interactionDistance = 2f;
}