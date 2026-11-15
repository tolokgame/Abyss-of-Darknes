
using System.Collections;
using UnityEngine;

public class PasswordBox : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float interactionDistance = 2.5f;
    [SerializeField] private KeyCode interactionKey = KeyCode.E;

    [Header("Password")]
    [SerializeField] private string correctPassword = "0824";
    [SerializeField] private PasswordUI passwordUI;

    [Header("Box Animation")]
    [SerializeField] private Transform lid;
    [SerializeField] private Vector3 openRotation = new Vector3(-100f, 0f, 0f);
    [SerializeField] private float animationDuration = 1f;

    [Header("Reward")]
    [SerializeField] private GameObject keyObject;

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    private bool isOpen;
    private bool isAnimating;

    private void Start()
    {
        if (lid != null)
        {
            closedRotation = lid.localRotation;
            openedRotation = Quaternion.Euler(openRotation);
        }

        if (keyObject != null)
            keyObject.SetActive(false);
    }

    private void Update()
    {
        if (player == null || isOpen || isAnimating)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        if (distance <= interactionDistance &&
            Input.GetKeyDown(interactionKey))
        {
            passwordUI.Show(this);
        }
    }

    public bool CheckPassword(string enteredPassword)
    {
        if (enteredPassword != correctPassword)
            return false;

        isOpen = true;

        StartCoroutine(OpenBox());

        return true;
    }

    private IEnumerator OpenBox()
    {
        isAnimating = true;

        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / animationDuration);
            t = t * t * (3f - 2f * t);

            lid.localRotation = Quaternion.Slerp(
                closedRotation,
                openedRotation,
                t
            );

            yield return null;
        }

        lid.localRotation = openedRotation;

        if (keyObject != null)
            keyObject.SetActive(true);

        isAnimating = false;
    }
}
