using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonColorHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image targetImage; // The image to modify
    public Color normalColor = Color.white; // Default color
    public Color highlightedColor = new Color(1f, 1f, 1f, 0.8f); // Lighter color when hovered
    public Color pressedColor = new Color(1f, 1f, 1f, 0.6f); // Even lighter color when pressed

    private static ButtonColorHighlighter selectedButton; // Keeps track of the selected button

    private void Start()
    {
        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetImage != null && selectedButton != this)
        {
            targetImage.color = highlightedColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null && selectedButton != this)
        {
            targetImage.color = normalColor;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = pressedColor;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (targetImage != null)
        {
            targetImage.color = highlightedColor;
            if (selectedButton != null && selectedButton != this)
            {
                selectedButton.ResetToNormal();
            }
            selectedButton = this;
        }
    }

    private void ResetToNormal()
    {
        if (targetImage != null)
        {
            targetImage.color = normalColor;
        }
    }
}

