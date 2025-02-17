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
    private bool isMoving = false; // Track movement

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

            if (movingUp)
            {
                // Move upwards until reaching finalYPosition
                if (currentY < finalYPosition)
                {
                    targetObject.transform.position += Vector3.up * moveSpeed * Time.deltaTime;
                }
                else
                {
                    isMoving = false; // Stop moving at finalYPosition
                }
            }
            else
            {
                // Move downwards until reaching startYPosition
                if (currentY > startYPosition)
                {
                    targetObject.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
                }
                else
                {
                    isMoving = false; // Stop moving at startYPosition
                }
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
