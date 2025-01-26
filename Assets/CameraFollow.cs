using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Het transform van de speler
    public float smoothSpeed = 0.125f; // De snelheid waarmee de camera beweegt (smoothing)
    public Vector3 offset = new Vector3(0, 2, -10); // Verplaatsing van de camera ten opzichte van de speler (kan aangepast worden)

    void FixedUpdate()
    {
        if (target != null) // Controleer of het doel (de speler) niet null is
        {
            Vector3 desiredPosition = target.position + offset; // Bepaal de gewenste positie van de camera op basis van de speler en de offset
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); // Maak een vloeiende overgang naar de gewenste positie
            transform.position = smoothedPosition; // Stel de nieuwe positie van de camera in
        }
    }
}