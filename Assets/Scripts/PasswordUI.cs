
using UnityEngine;
using TMPro;

public class PasswordUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject passwordPanel;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TMP_Text messageText;

    private PasswordBox currentBox;

    private void Start()
    {
        passwordPanel.SetActive(false);
    }

    public void Show(PasswordBox box)
    {
        currentBox = box;

        passwordPanel.SetActive(true);

        passwordInput.text = "";
        messageText.text = "";

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        passwordInput.ActivateInputField();
    }

    public void SubmitPassword()
    {
        if (currentBox == null)
            return;

        bool correct = currentBox.CheckPassword(
            passwordInput.text
        );

        if (correct)
        {
            messageText.text = "Шкатулка відкрита!";
            passwordPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            messageText.text = "Неправильний пароль!";
            passwordInput.text = "";

            passwordInput.ActivateInputField();
        }
    }

    public void Close()
    {
        passwordPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentBox = null;
    }
}
