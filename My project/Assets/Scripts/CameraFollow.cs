using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player’s transform
    public Vector3 offset = new Vector3(0f, 2f, -10f);  // Offset from the player
    public float smoothSpeed = 5f;  // How smoothly the camera follows

    void LateUpdate()
    {
        if (player == null) return;  // Ensure player exists before updating

        // Target position the camera should move toward
        Vector3 targetPosition = player.position + offset;

        // Smoothly interpolate between the current position and target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}

