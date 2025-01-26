using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;							// Hoeveel kracht er wordt toegevoegd wanneer de speler springt.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Het percentage van de maximale snelheid toegepast op het kruipen. 1 = 100%
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// Hoeveel de beweging moet worden gesmeed (versoepeld)
    [SerializeField] private bool m_AirControl = false;							// Of de speler de beweging kan sturen terwijl hij springt
    [SerializeField] private LayerMask m_WhatIsGround;							// Een masker om te bepalen wat grond is voor de speler
    [SerializeField] private Transform m_GroundCheck;							// Een positie om te controleren of de speler op de grond staat.
    [SerializeField] private Transform m_CeilingCheck;							// Een positie om te controleren voor een plafond
    [SerializeField] private Collider2D m_CrouchDisableCollider;				// Een collider die uitgeschakeld wordt tijdens het kruipen

    const float k_GroundedRadius = .2f; // Straal van de overlapcirkel om te controleren of de speler op de grond staat
    private bool m_Grounded;            // Of de speler op de grond staat
    const float k_CeilingRadius = .2f; // Straal van de overlapcirkel om te controleren of de speler rechtop kan staan
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;  // Om te bepalen welke kant de speler op kijkt
    private Vector3 m_Velocity = Vector3.zero;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent; // Event voor wanneer de speler landt

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent; // Event voor wanneer de speler begint of stopt met kruipen
    private bool m_wasCrouching = false; // Bijhouden of de speler bezig is met kruipen

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        if (OnCrouchEvent == null)
            OnCrouchEvent = new BoolEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // De speler is op de grond als een cirkelcast naar de grondcheckpositie iets raakt dat als grond is gemarkeerd
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke(); // Roept het landingsevent aan wanneer de speler op de grond komt
            }
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        // Als de speler niet kruipt, controleren of de speler kan opstaan
        if (!crouch)
        {
            // Als er een plafond is dat het rechtop staan van de speler blokkeert, blijft de speler kruipen
            if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
            {
                crouch = true;
            }
        }

        // Alleen de speler besturen als hij op de grond staat of als airControl is ingeschakeld
        if (m_Grounded || m_AirControl)
        {
            // Als de speler kruipt
            if (crouch)
            {
                if (!m_wasCrouching)
                {
                    m_wasCrouching = true;
                    OnCrouchEvent.Invoke(true); // Roept het crouch-event aan
                }

                // Verminder de snelheid door de crouchSpeed-multiplier toe te passen
                move *= m_CrouchSpeed;

                // Zet de collider uit tijdens het kruipen
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;
            } 
            else
            {
                // Zet de collider aan wanneer de speler niet meer kruipt
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                if (m_wasCrouching)
                {
                    m_wasCrouching = false;
                    OnCrouchEvent.Invoke(false); // Roept het crouch-event aan wanneer de speler stopt met kruipen
                }
            }

            // Beweeg de speler door de doel snelheid te berekenen
            Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
            // Versoepel de snelheid en pas deze toe op de speler
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // Als de speler naar rechts beweegt en de speler kijkt naar links...
            if (move > 0 && !m_FacingRight)
            {
                // Draai de speler om
                Flip();
            }
            // Als de speler naar links beweegt en de speler kijkt naar rechts...
            else if (move < 0 && m_FacingRight)
            {
                // Draai de speler om
                Flip();
            }
        }

        // Als de speler wil springen
        if (m_Grounded && jump)
        {
            // Voeg verticale kracht toe om de speler te laten springen
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void Flip()
    {
        // Wissel de kant waarop de speler wordt weergegeven
        m_FacingRight = !m_FacingRight;

        // Verander de x-schaal van de speler met -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}