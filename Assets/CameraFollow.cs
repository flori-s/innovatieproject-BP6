using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player’s transform
    public float smoothSpeed = 0.125f; // Smoothing speed for camera movement
    public Vector3 offset = new Vector3(0, 2, -10); // Adjust as needed
    void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}