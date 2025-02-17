using UnityEngine;
using UnityEngine.UI;

public class WindowsButton : MonoBehaviour
{
    public GameObject button;          // UI Button reference
    public GameObject targetObject; // The object to move
    public float moveSpeed = 2f;   // Speed of movement
    public float finalYPosition = 5f; // The final Y position to stop moving up

    private float startYPosition;  // The starting Y position
    private bool movingUp = false;  // Direction toggle
    private bool isMoving = true; // Track movement

    void Start()
    {
        if (targetObject != null)
        {
            startYPosition = targetObject.transform.position.y; // Store initial Y position
        }
        else
        {
            Debug.LogWarning("Target Object is not assigned!");
        }

        button.GetComponent<Button>().onClick.AddListener(() => ToggleMovement());
    }

    void Update()
    {
        if (isMoving && targetObject != null)
        {
            float currentY = targetObject.transform.position.y;
            float newY;

            if (movingUp)
            {
                // Move upwards and clamp the value
                newY = Mathf.Min(currentY + moveSpeed * Time.deltaTime, finalYPosition);
            }
            else
            {
                // Move downwards and clamp the value
                newY = Mathf.Max(currentY - moveSpeed * Time.deltaTime, startYPosition);
            }

            targetObject.transform.position = new Vector3(
                targetObject.transform.position.x,
                newY,
                targetObject.transform.position.z
            );

            // Stop movement if the target is reached
            if (newY == finalYPosition || newY == startYPosition)
            {
                isMoving = false;
            }
        }
    }


    void ToggleMovement()
    {
        if (targetObject != null)
        {
            isMoving = true;  // Start movement
            movingUp = !movingUp; // Toggle direction
        }
    }
}
