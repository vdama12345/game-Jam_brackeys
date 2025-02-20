using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DrawOnCanvas : MonoBehaviour
{
    public int brushSize = 10; // Base brush size (adjustable in Inspector)
    public Color brushColor = Color.black; // Default brush color
    public float smoothness = 0.1f; // Smoothing factor (between 0 and 1)

    private Texture2D drawingTexture;
    private RectTransform rectTransform;
    private Image image;

    private Vector2 lastPoint;
    private bool isEraserMode = false; // Flag for eraser mode

    private List<Texture2D> drawingHistory = new List<Texture2D>(); // To store history for undo

    // UI elements for highlighting (the squares around buttons)
    public Image penButtonHighlight;
    public Image eraserButtonHighlight;

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        if (image == null)
        {
            Debug.LogError("No Image component found!");
            return;
        }

        // Create a blank white texture
        drawingTexture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        Color[] fillColor = new Color[drawingTexture.width * drawingTexture.height];

        for (int i = 0; i < fillColor.Length; i++)
            fillColor[i] = Color.white; // Fill with white

        drawingTexture.SetPixels(fillColor);
        drawingTexture.Apply();

        // Assign to the Image component
        image.sprite = Sprite.Create(drawingTexture, new Rect(0, 0, 512, 512), new Vector2(0.5f, 0.5f));

        // Make sure pen button is selected by default
        SetPenMode();
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Left-click or tablet to draw/erase
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint))
            {
                float pressure = Input.touchCount > 0 ? Input.GetTouch(0).pressure : 1f; // Pressure from touch or tablet
                if (isEraserMode)
                {
                    Erase(localPoint); // Erase if in eraser mode
                }
                else
                {
                    Draw(localPoint, pressure); // Draw if in pen mode
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // Reset last point when the mouse is lifted
        {
            lastPoint = Vector2.zero; // Reset to avoid linking previous points
        }

        if (Input.GetKeyDown(KeyCode.Z) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) // Ctrl+Z for undo
        {
            Undo();
        }
    }

    // Draw the line in pen mode
    void Draw(Vector2 localPoint, float pressure)
    {
        // Convert local UI position to texture coordinates
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        int x = (int)(((localPoint.x + width / 2) / width) * drawingTexture.width);
        int y = (int)(((localPoint.y + height / 2) / height) * drawingTexture.height);

        // Adjust brush size based on pressure
        int dynamicBrushSize = Mathf.Clamp((int)(brushSize * pressure), 1, brushSize * 2);

        // Draw smooth lines and apply anti-aliasing
        if (lastPoint != Vector2.zero)
        {
            DrawLine(lastPoint, new Vector2(x, y), dynamicBrushSize);
        }

        lastPoint = new Vector2(x, y); // Update last point

        drawingTexture.Apply(); // Apply changes to texture

        // Save state after drawing
        SaveState();
    }

    // Erase part of the drawing
    void Erase(Vector2 localPoint)
    {
        // Convert local UI position to texture coordinates
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        int x = (int)(((localPoint.x + width / 2) / width) * drawingTexture.width);
        int y = (int)(((localPoint.y + height / 2) / height) * drawingTexture.height);

        // Erase in the region of the brush
        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                if (i * i + j * j <= brushSize * brushSize) // Circular eraser shape
                {
                    int px = Mathf.Clamp(x + i, 0, drawingTexture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, drawingTexture.height - 1);
                    drawingTexture.SetPixel(px, py, Color.white); // Set to white (eraser effect)
                }
            }
        }

        drawingTexture.Apply(); // Apply changes to texture

        // Save state after erasing
        SaveState();
    }

    // Draw a line between two points
    void DrawLine(Vector2 start, Vector2 end, int brushSize)
    {
        Vector2 direction = end - start;
        float distance = direction.magnitude;
        direction.Normalize();

        // Anti-aliasing (blend between full transparency and brush color)
        for (float t = 0; t < distance; t += smoothness)
        {
            Vector2 point = start + direction * t;
            int x = Mathf.Clamp((int)point.x, 0, drawingTexture.width - 1);
            int y = Mathf.Clamp((int)point.y, 0, drawingTexture.height - 1);

            // Apply anti-aliasing by blending pixels
            Color color = brushColor * 0.8f; // Full color
            drawingTexture.SetPixel(x, y, Color.Lerp(drawingTexture.GetPixel(x, y), color, 0.8f));
        }
    }

    // Save the current drawing state
    void SaveState()
    {
        Texture2D textureClone = new Texture2D(drawingTexture.width, drawingTexture.height);
        textureClone.SetPixels(drawingTexture.GetPixels());
        textureClone.Apply();
        drawingHistory.Add(textureClone);
        Debug.Log("State saved, history count: " + drawingHistory.Count);
    }

    // Undo the last drawing action
    void Undo()
    {
        if (drawingHistory.Count > 1)
        {
            drawingHistory.RemoveAt(drawingHistory.Count - 1); // Remove the most recent state
            Texture2D previousState = drawingHistory[drawingHistory.Count - 1]; // Get the previous state
            drawingTexture.SetPixels(previousState.GetPixels());
            drawingTexture.Apply(); // Apply the texture
            Debug.Log("Undo performed, history count: " + drawingHistory.Count);
        }
        else
        {
            Debug.Log("No more states to undo.");
        }
    }

    // Switch to pen mode
    public void SetPenMode()
    {
        isEraserMode = false; // Set to pen mode
        penButtonHighlight.color = Color.green; // Highlight the pen button
        eraserButtonHighlight.color = Color.white; // Reset the eraser button highlight
    }

    // Switch to eraser mode
    public void SetEraserMode()
    {
        isEraserMode = true; // Set to eraser mode
        penButtonHighlight.color = Color.white; // Reset the pen button highlight
        eraserButtonHighlight.color = Color.green; // Highlight the eraser button
    }

    // Called when the undo button is clicked
    public void OnUndoButtonClicked()
    {
        Undo();
    }
}


