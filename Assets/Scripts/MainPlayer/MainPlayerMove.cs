using UnityEngine;

public class MainPlayerMove : MonoBehaviour
{
    private bool canMove = true;
    private Vector3 _initialPosition = Vector3.zero;

    // Android
    [Header("Android")]
    [SerializeField] private Joystick Stick = null;

    // Values
    [Header("Player Values")]
    [SerializeField] private float PlayerSpeed = 5f;
    [SerializeField] private float PlayerJumpForce = 12f;
    [SerializeField] private int NumberOfJumps = 1;
    private int _currentJump = 0;
    private Transform _transform = null;
    private Vector3 _direction = Vector3.zero;
    private Rigidbody _playerRb = null;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private AudioClip JumpSFX = null;
    private PlayerData _playerData = null;

    void Awake()
    {
        _playerData = GetComponent<PlayerData>();

        _transform = transform;
        _playerRb = GetComponent<Rigidbody>();
        _currentJump = NumberOfJumps;
        _initialPosition = this.transform.position;

#if UNITY_STANDALONE // Desactivar joystick en PC
        if (Stick != null) Stick.gameObject.SetActive(false); 
#else // Activa el objeto de joystick en móvil
        if (Stick != null) Stick.gameObject.SetActive(true);
#endif
    }

    void Update()
    {
        if (canMove)
        {
#if UNITY_STANDALONE // Si está en PC usa las teclas
            _direction.x = Input.GetAxis("Horizontal");
            _direction.z = Input.GetAxis("Vertical");

            if (Input.GetKeyDown(KeyCode.Space)) JumpAction();

#else // Si está en móvil usa el joystick
            if (Stick != null)
            {
                _direction.x = Stick.Horizontal;
                _direction.z = Stick.Vertical;
            }
#endif


            _playerRb.velocity = new Vector3(_direction.x * PlayerSpeed, _playerRb.velocity.y, _direction.z * PlayerSpeed);


            if (this.transform.position.y < 2) _transform.position = _initialPosition;
        }
    }

    public void SetCanMove(bool enableMoveOrNot) => canMove = enableMoveOrNot;
    public bool GetCanMove() => canMove;

    public void JumpAction()
    {
        if (_currentJump > 0)
        {
            _currentJump--;
            _playerRb.AddForce(Vector3.up * PlayerJumpForce, ForceMode.Impulse);

            _playerData.PlaySoundEffect(JumpSFX);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        { 
            _currentJump = NumberOfJumps; 
        }
    }
}
