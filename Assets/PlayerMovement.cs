using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller; // Reference to CharacterController2D
    public float runSpeed = 40f;             // Movement speed
    private float m_HorizontalMove = 0f;     // Horizontal input
    private bool m_Jump = false;             // Jump input
    private bool m_Crouch = false;           // Crouch input

    // Update is called once per frame
    void Update()
    {
        // Get horizontal input (-1 for left, 1 for right, 0 for no input)
        m_HorizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Check if the jump button (default: spacebar) is pressed
        if (Input.GetButtonDown("Jump"))
        {
            m_Jump = true;
        }

        // Check if the crouch button (default: left control) is held
        m_Crouch = Input.GetKey(KeyCode.LeftControl);
    }

    // FixedUpdate is called at a fixed interval (used for physics)
    void FixedUpdate()
    {
        // Pass the input to the CharacterController2D script
        controller.Move(m_HorizontalMove * Time.fixedDeltaTime, m_Crouch, m_Jump);

        // Reset the jump flag for the next frame
        m_Jump = false;
    }
}