using UnityEngine;
using UnityEngine.UI;

public class BallRing : MonoBehaviour
{
    [SerializeField] private Transform _initialPosition;

    [SerializeField] private float maxJump = 10f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Joystick joystick;

    [SerializeField] private float currentJump;
    [SerializeField] private Slider SliderCurrentJump;

    private bool canJump = true;
    private bool isPressed = false;

    private bool isGrounded = false;
    private Rigidbody rb;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private AudioClip JumpSFX = null;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        this.transform.position = _initialPosition.position;
    }

    private void Update()
    {
#if UNITY_STANDALONE
        // PC
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed;

        // Salto
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space)) currentJump = 0;

            if (Input.GetKey(KeyCode.Space)) currentJump = Mathf.Clamp(currentJump + Time.deltaTime * maxJump, 0, maxJump);

            if (Input.GetKeyUp(KeyCode.Space)) Jump();
        }

#else
        // Movil
        float moveX = joystick.Horizontal * moveSpeed;
        float moveZ = joystick.Vertical * moveSpeed;

        // Salto
        if (isGrounded)
        {
            if (isPressed)
            {
                currentJump = Mathf.Clamp(currentJump + Time.deltaTime * maxJump, 0, maxJump);
            }

            if (!isPressed && currentJump > 0) // Cuando el botón se suelta
            {
                Jump();
            }
        }
#endif
        // Movimiento horizontal
        rb.velocity = new Vector3(moveX, rb.velocity.y, moveZ);

        SliderCurrentJump.value = currentJump;
    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerData.PlaySoundEffect(JumpSFX);

            rb.velocity = new Vector3(rb.velocity.x, currentJump, rb.velocity.z);
            currentJump = 0;
        }
    }


    // Detecta si está tocando el suelo
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = true;
    }

    // Detecta si deja de tocar el suelo
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
            isGrounded = false;
    }

    // Métodos para el botón UI
    public void OnJumpButtonDown()
    {
        isPressed = true; // Simula mantener presionado el botón
    }

    public void OnJumpButtonUp()
    {
        isPressed = false; // Simula soltar el botón
    }
}
