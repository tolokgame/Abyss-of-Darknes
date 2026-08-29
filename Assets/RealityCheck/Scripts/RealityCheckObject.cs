using UnityEngine;

public class RealityCheckObject : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float disappearDistance = 2.5f;
    [SerializeField] private float appearDistance = 3.5f;

    private bool isHidden = false;

    private void Update()
    {
        if (player == null)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        if (!isHidden && distance <= disappearDistance)
        {
            isHidden = true;
            HideObject();
        }

        if (isHidden && distance >= appearDistance)
        {
            isHidden = false;
            ShowObject();
        }
    }

    private void HideObject()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }
    }

    private void ShowObject()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }
    }
}