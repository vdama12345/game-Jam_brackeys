using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CanvasScroller : MonoBehaviour
{
    public ScrollRect scrollRect;  // Assign the ScrollRect component
    public Transform player;       // Assign the player's Transform
    public float scrollSpeed = 50f; // Speed of scrolling when jumping
    public float fallScrollSpeed = 30f; // Speed of scrolling when falling
    public float resetSpeed = 100f; // Speed of returning to default position
    public Vector2 defaultPosition = Vector2.zero; // Default scroll position

    private float lastPlayerY;
    private RectTransform contentRectTransform;
    private bool isInTrigger = false; // Only scroll while inside trigger
    private bool hasJumped = false;

    void Start()
    {
        lastPlayerY = player.position.y; // Store initial Y position
        contentRectTransform = scrollRect.content;
        defaultPosition = contentRectTransform.anchoredPosition; // Store default position
    }

    void Update()
    {
        if (isInTrigger)
        {
            float currentPlayerY = player.position.y;
            float deltaY = currentPlayerY - lastPlayerY;

            // If the player is moving up, scroll content down
            if (deltaY > 0)
            {
                ScrollCanvas(-deltaY * scrollSpeed);
                hasJumped = true; // Mark that the player has jumped
            }
            // If falling and has previously jumped, scroll content up
            else if (deltaY < 0 && hasJumped)
            {
                ScrollCanvas(-deltaY * fallScrollSpeed);
            }

            lastPlayerY = currentPlayerY; // Update last Y position
        }
        else
        {
            // Smoothly return to default position when outside trigger
            contentRectTransform.anchoredPosition = Vector2.Lerp(
                contentRectTransform.anchoredPosition, defaultPosition, Time.deltaTime * resetSpeed
            );
        }
    }

    void ScrollCanvas(float amount)
    {
        float targetY = contentRectTransform.anchoredPosition.y + amount;
        targetY = Mathf.Clamp(targetY, -contentRectTransform.rect.height, 0f);
        contentRectTransform.anchoredPosition = new Vector2(contentRectTransform.anchoredPosition.x, targetY);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == player)
        {
            isInTrigger = true; // Enable scrolling when inside trigger
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == player)
        {
            isInTrigger = false; // Disable scrolling and reset to default
        }
    }
}







