using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller; // Verwijzing naar de CharacterController2D
    public float runSpeed = 40f;             // Bewegsnelheid
    private float m_HorizontalMove = 0f;     // Horizontale input
    private bool m_Jump = false;             // Springinvoer
    private bool m_Crouch = false;           // Kruipen invoer

    // Update wordt één keer per frame aangeroepen
    void Update()
    {
        // Haal horizontale input op (-1 voor links, 1 voor rechts, 0 voor geen input)
        m_HorizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Controleer of de sprongknop (standaard: spatiebalk) is ingedrukt
        if (Input.GetButtonDown("Jump"))
        {
            m_Jump = true;
        }

        // Controleer of de kruipknop (standaard: linker Ctrl) ingedrukt wordt
        m_Crouch = Input.GetKey(KeyCode.LeftControl);
    }

    // FixedUpdate wordt op een vast interval aangeroepen (gebruikt voor fysica)
    void FixedUpdate()
    {
        // Geef de input door aan de CharacterController2D script
        controller.Move(m_HorizontalMove * Time.fixedDeltaTime, m_Crouch, m_Jump);

        // Zet de springvlag terug voor de volgende frame
        m_Jump = false;
    }
}