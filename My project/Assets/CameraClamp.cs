using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;

    void Start()
    {
        mainCamera = Camera.main;
        SetBounds();
    }

    void Update()
    {
        RestrictMovement();
    }

    void SetBounds()
    {
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));

        minX = bottomLeft.x;
        maxX = topRight.x;
        minY = bottomLeft.y;
        maxY = topRight.y;
    }

    void RestrictMovement()
    {
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);
        transform.position = clampedPosition;
    }
}
