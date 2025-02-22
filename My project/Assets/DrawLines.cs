using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DrawOnCanvas : MonoBehaviour
{
    public int brushSize = 10;
    public Color brushColor = Color.black;
    public float smoothness = 0.1f;

    private Texture2D drawingTexture;
    private RectTransform rectTransform;
    private Image image;

    private Vector2 lastPoint;
    private bool isEraserMode = false;

    public Image penButtonHighlight;
    public Image eraserButtonHighlight;

    public GameObject linePrefab; // Prefab for line colliders
    private LineRenderer currentLineRenderer;
    private PolygonCollider2D currentCollider;
    private List<Vector2> colliderPoints;

    private bool isDrawing = false; // Track whether the user is actively drawing

    public Button clearButton; // Reference to the clear button

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        if (image == null)
        {
            Debug.LogError("No Image component found!");
            return;
        }

        drawingTexture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        Color[] fillColor = new Color[drawingTexture.width * drawingTexture.height];

        for (int i = 0; i < fillColor.Length; i++)
            fillColor[i] = Color.white;

        drawingTexture.SetPixels(fillColor);
        drawingTexture.Apply();

        image.sprite = Sprite.Create(drawingTexture, new Rect(0, 0, 512, 512), new Vector2(0.5f, 0.5f));

        SetPenMode();

        // Add listener to the clear button
        clearButton.onClick.AddListener(ClearCanvas);
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) // Mouse is down
        {
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, Input.mousePosition, Camera.main, out localPoint))
            {
                float pressure = Input.touchCount > 0 ? Input.GetTouch(0).pressure : 1f;

                if (!isDrawing)
                {
                    // Start a new line and reset the points when mouse is down
                    StartNewLine();
                }

                if (isEraserMode)
                {
                    Erase(localPoint);
                }
                else
                {
                    Draw(localPoint, pressure);
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) // Mouse is released
        {
            FinishCurrentLine();
            lastPoint = Vector2.zero;
            isDrawing = false; // Stop drawing when mouse is released
        }
    }

    void Draw(Vector2 localPoint, float pressure)
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        int x = (int)(((localPoint.x + width / 2) / width) * drawingTexture.width);
        int y = (int)(((localPoint.y + height / 2) / height) * drawingTexture.height);

        int dynamicBrushSize = Mathf.Clamp((int)(brushSize * pressure), 1, brushSize * 2);

        if (lastPoint != Vector2.zero)
        {
            DrawLine(lastPoint, new Vector2(x, y), dynamicBrushSize);
        }

        // Convert local point to world point and add to collider
        Vector2 worldPoint = ConvertToWorldPoint(localPoint);
        AddPointToCollider(worldPoint);

        lastPoint = new Vector2(x, y);
        drawingTexture.Apply();
    }

    void Erase(Vector2 localPoint)
    {
        // Erase pixels in the texture
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        int x = (int)(((localPoint.x + width / 2) / width) * drawingTexture.width);
        int y = (int)(((localPoint.y + height / 2) / height) * drawingTexture.height);

        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                if (i * i + j * j <= brushSize * brushSize)
                {
                    int px = Mathf.Clamp(x + i, 0, drawingTexture.width - 1);
                    int py = Mathf.Clamp(y + j, 0, drawingTexture.height - 1);
                    drawingTexture.SetPixel(px, py, Color.white);
                }
            }
        }

        // Erase line prefabs (colliders and lines)
        foreach (Transform child in transform) // Iterate through all line prefabs (children)
        {
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();
            PolygonCollider2D polygonCollider = child.GetComponent<PolygonCollider2D>();

            if (lineRenderer != null && polygonCollider != null)
            {
                // Check if any of the collider points are within the eraser radius
                bool shouldDeleteLine = false;
                foreach (Vector2 point in colliderPoints)
                {
                    if (Vector2.Distance(localPoint, point) <= brushSize)
                    {
                        shouldDeleteLine = true;
                        break; // Stop if any point is within the eraser radius
                    }
                }

                // If any point of the line is within the erase area, destroy the line and its collider
                if (shouldDeleteLine)
                {
                    Destroy(child.gameObject); // Destroy the whole line prefab (collider + LineRenderer)
                }
            }
        }

        drawingTexture.Apply();
    }

    void DrawLine(Vector2 start, Vector2 end, int brushSize)
    {
        Vector2 direction = end - start;
        float distance = direction.magnitude;
        direction.Normalize();

        for (float t = 0; t < distance; t += smoothness)
        {
            Vector2 point = start + direction * t;
            int x = Mathf.Clamp((int)point.x, 0, drawingTexture.width - 1);
            int y = Mathf.Clamp((int)point.y, 0, drawingTexture.height - 1);

            Color color = brushColor * 0.8f;
            drawingTexture.SetPixel(x, y, Color.Lerp(drawingTexture.GetPixel(x, y), color, 0.8f));
        }
    }

    void AddPointToCollider(Vector2 worldPoint)
    {
        // Adjust position slightly to match the visual line
        Vector2 adjustedPoint = new Vector2(worldPoint.x + 0.27f, worldPoint.y + 1.268f); // Adjust this value as needed

        // Clamp the point within the image boundaries
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;
        adjustedPoint.x = Mathf.Clamp(adjustedPoint.x, -width / 2, width / 2);
        adjustedPoint.y = Mathf.Clamp(adjustedPoint.y, -height / 2, height / 2);

        // Initialize the collider and points list if not already done
        if (colliderPoints == null)
        {
            colliderPoints = new List<Vector2>();
            GameObject newLine = Instantiate(linePrefab, transform);
            currentLineRenderer = newLine.GetComponent<LineRenderer>();
            currentCollider = newLine.GetComponent<PolygonCollider2D>(); // Using PolygonCollider2D
        }

        // Add a new point to the list if it's far enough from the last point
        if (colliderPoints.Count == 0 || Vector2.Distance(colliderPoints[colliderPoints.Count - 1], adjustedPoint) > 0.01f)
        {
            colliderPoints.Add(adjustedPoint);

            // Update the LineRenderer positions
            currentLineRenderer.positionCount = colliderPoints.Count;
            currentLineRenderer.SetPosition(colliderPoints.Count - 1, adjustedPoint);

            // Update the PolygonCollider2D to match the points
            currentCollider.SetPath(0, colliderPoints.ToArray());
        }
    }

    void FinishCurrentLine()
    {
        if (currentCollider != null && colliderPoints.Count > 2)
        {
            // Set the collider's path to the world points
            currentCollider.SetPath(0, colliderPoints.ToArray());
        }
    }

    Vector2 ConvertToWorldPoint(Vector2 localPoint)
    {
        // Convert from local space (canvas space) to world space
        Vector3 worldPoint = rectTransform.TransformPoint(localPoint);
        return new Vector2(worldPoint.x, worldPoint.y);
    }

    void StartNewLine()
    {
        // Mark that the user is starting a new line
        isDrawing = true;

        // Clear the old collider and points
        colliderPoints = new List<Vector2>();
        GameObject newLine = Instantiate(linePrefab, transform);
        currentLineRenderer = newLine.GetComponent<LineRenderer>();
        currentCollider = newLine.GetComponent<PolygonCollider2D>();
    }

    public void SetPenMode()
    {
        isEraserMode = false;
        penButtonHighlight.color = Color.green;
        eraserButtonHighlight.color = Color.white;
    }

    public void SetEraserMode()
    {
        isEraserMode = true;
        penButtonHighlight.color = Color.white;
        eraserButtonHighlight.color = Color.green;
    }

    // Clear button method
    public void ClearCanvas()
    {
        // Clear all line prefabs
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Clear the drawing texture by setting all pixels to white
        Color[] fillColor = new Color[drawingTexture.width * drawingTexture.height];
        for (int i = 0; i < fillColor.Length; i++)
        {
            fillColor[i] = Color.white;
        }
        drawingTexture.SetPixels(fillColor);
        drawingTexture.Apply();
    }
}
