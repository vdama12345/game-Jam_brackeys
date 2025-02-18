using UnityEngine;
using UnityEngine.UI;

public class ToggleScreen : MonoBehaviour
{
    public GameObject targetObject; // Assign in Inspector
    public Button toggleButton; // Assign in Inspector

    void Start()
    {
        if (toggleButton != null)
        {
            toggleButton.onClick.AddListener(ToggleActiveState);
        }
    }

    void ToggleActiveState()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}

