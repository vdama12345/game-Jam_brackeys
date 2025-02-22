using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PasswordChecker : MonoBehaviour
{
    // Public fields for the UI elements
    public GameObject passwordScreen;
    public TMP_InputField passwordInputField; // The input field for the password
    public Button submitButton;           // The button to submit the password
    public GameObject successScreen;      // The screen to activate on successful password entry
    public TMP_Text errorMessageText;         // Text to show error if the password is incorrect

    // Predefined password
    private string correctPassword = "M3M-M3m0r1E";

    void Start()
    {
        // Add listener for the submit button
        submitButton.onClick.AddListener(CheckPassword);
    }

    // Method to check the entered password
    void CheckPassword()
    {
        string enteredPassword = passwordInputField.text; // Get the text from the input field

        if (enteredPassword == correctPassword)
        {
            passwordScreen.SetActive(false);

            // Activate the success screen
            successScreen.SetActive(true);

            // Hide error message if password is correct
            errorMessageText.gameObject.SetActive(false);
        }
        else
        {
            // Show error message if password is incorrect
            errorMessageText.text = "Incorrect password! Please try again.";
            errorMessageText.gameObject.SetActive(true);
        }
    }
}
