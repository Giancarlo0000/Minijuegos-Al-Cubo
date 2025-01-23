using UnityEngine;

public class PlayerMaze : MonoBehaviour
{
    [SerializeField] private Joystick Stick = null;
    [SerializeField] private float PlayerSpeed = 5f;
    [SerializeField] private float SlideFactor = 0.99f;
    [SerializeField] private float DirectionSmoothness = 0.1f;

    [SerializeField] private Transform _initialPosition = null;
    [SerializeField] private GameObject Joystick = null, MazeGame = null;


    private Rigidbody _rigidbody = null;
    private Vector3 _direction = Vector3.zero;
    private Vector3 _currentVelocity = Vector3.zero;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private AudioClip wrongSFX = null;
    [SerializeField] private AudioClip doneGameSFX = null;
    private PlayerData _playerData = null;

    private void Awake()
    {
        _playerData = FindAnyObjectByType<PlayerData>();
        _rigidbody = GetComponent<Rigidbody>();


#if UNITY_STANDALONE
#else
        Stick.gameObject.SetActive(true);
#endif
    }

    private void Start()
    {
        _initialPosition.position = this.transform.position;
    }


    private void OnEnable()
    {
        // Cada vez que se habilita, se reinicia el juego
        ResetGame();

        SetEnableDisableGame(true);
    }

    private void OnDisable()
    {
        ResetGame();

        SetEnableDisableGame(false);
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
#if UNITY_STANDALONE
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");
#else
        _direction.x = Stick.Horizontal;
        _direction.z = Stick.Vertical;
#endif

        if (_direction.magnitude > 0.1f)
        {
            Vector3 desiredVelocity = _direction.normalized * PlayerSpeed;
            _currentVelocity = Vector3.Lerp(_currentVelocity, desiredVelocity, DirectionSmoothness);
            _rigidbody.velocity = new Vector3(_currentVelocity.x, _rigidbody.velocity.y, _currentVelocity.z);
        }
        else
        {
            Vector3 slidingVelocity = new Vector3(_rigidbody.velocity.x * SlideFactor, _rigidbody.velocity.y, _rigidbody.velocity.z);
            _rigidbody.velocity = slidingVelocity;
        }
    }


    public void ResetGame() 
    {
        this.transform.position = _initialPosition.position;
        _rigidbody.velocity = Vector3.zero;
    }

    private void WinGame()
    {
        // Marca el microjuego como ganado.
        FindObjectOfType<AppearGameScreen>().MarkGameAsWon(3);

        // Desactiva los objetos
        SetEnableDisableGame(false);

        // Cierra el panel del microjuego
        FindObjectOfType<AppearGameScreen>().CloseMicrogameWindow();
    }
    public void SetEnableDisableGame(bool setEnableDisableGame)
    {
#if UNITY_STANDALONE
#else
        if (Joystick != null)
            Joystick.SetActive(setEnableDisableGame);
#endif
        if (MazeGame != null)
            MazeGame.SetActive(setEnableDisableGame);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Si el player toca el tag DefeatMaze, se reinicia el juego
        if (other.CompareTag("DefeatMaze"))
        {
            _playerData.PlaySoundEffect(wrongSFX);
            ResetGame();
        }

        // Si el player toca el tag WinMaze, marca el microjuego como ganado y lo cierra
        if (other.CompareTag("WinMaze"))
        {
            _playerData.PlaySoundEffect(doneGameSFX);
            WinGame();
        }
    }
}