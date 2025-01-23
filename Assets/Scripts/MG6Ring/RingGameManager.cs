using UnityEngine;
using UnityEngine.UI;

public class RingGameManager : MonoBehaviour
{
    private bool _inGame = false;
    private bool _canStart = false;

    [Header("SELECT")]
    [SerializeField] private GameObject RingMG6;
    [SerializeField] private Button ActionButton = null;
    [SerializeField] private Button CloseButton = null;
    [SerializeField] private Button JumpGameButton = null;
    [SerializeField] private Joystick Joystick = null;
    [SerializeField] private Slider JumpSlider = null;
    private AppearGameScreen _appearGameScreen = null;

    [Space(10)]
    [Header("Score")]
    private int _currentPoints = 0;
    [SerializeField] private int MaxPointsToWin = 0;
    [SerializeField] private Text scoreTxt = null;

    [Space(10)]
    [Header("Timer")]
    [SerializeField] private Text timeTxt = null;
    [SerializeField] private int initialTime = 10;
    private float _currentTime = 0;
    private float _timeCounter = 1f;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private AudioClip addPointSFX = null;
    [SerializeField] private AudioClip resetSFX = null;

    private void Awake()
    {
        SetCurrentTime(initialTime);
        _appearGameScreen = FindObjectOfType<AppearGameScreen>();
        
        CloseButton.onClick.AddListener(() => CloseGameWindow());
    }

    void Update()
    {
        if (_currentPoints >= MaxPointsToWin)
        {
            _appearGameScreen.MarkGameAsWon(6);
            CloseGameWindow();
        }

        if (_inGame)
        {
            _timeCounter -= Time.deltaTime;

            if (_timeCounter <= 0f)
            {
                _timeCounter = 1f;
                _currentTime--;
                ActualizarTimeTxt();
            }

            if (_currentTime <= 0)
            {
                playerData.PlaySoundEffect(resetSFX);
                CloseGameWindow();
            }
        }

        if (_canStart == true)
        {
#if UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.E)) StartGame();
#else
            if (ActionButton != null)
            {
                ActionButton.onClick.RemoveAllListeners();
                ActionButton.onClick.AddListener(() => StartGame());
            }
#endif
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_appearGameScreen.IsGameWon(6))
        {
            _canStart = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canStart = false;
            _inGame = false;

            ActionButton.onClick.RemoveAllListeners();
        }
    }

    public void AddPoint()
    {
        playerData.PlaySoundEffect(addPointSFX);

        _currentPoints++;
        SetCurrentTime(initialTime);
        
        ActualizarTimeTxt();
        ActualizarScoreTxt();
    }

    public void CloseGameWindow()
    {
        _appearGameScreen.CloseMicrogameWindow();
        _inGame = false;

        SetGameObjectsActive(false);

        ReiniciarJuego();

    }

    private void StartGame()
    {
        if (!_inGame)
        {
            ReiniciarJuego();
            SetGameObjectsActive(true);
        }
    }

    private void SetGameObjectsActive(bool isActive)
    {
        RingMG6.SetActive(isActive);
        timeTxt.gameObject.SetActive(isActive);
        scoreTxt.gameObject.SetActive(isActive);
        JumpSlider.gameObject.SetActive(isActive);

#if UNITY_STANDALONE
#else
        JumpGameButton.gameObject.SetActive(isActive);
        Joystick.gameObject.SetActive(isActive);
#endif
    }

    private void ActualizarScoreTxt() => scoreTxt.text = $"Score: {_currentPoints} / {MaxPointsToWin}";
    private void ActualizarTimeTxt() => timeTxt.text = $"Tiempo restante: {_currentTime}";
    private void SetCurrentTime(float time) => _currentTime = time;

    private void ReiniciarJuego()
    {
        SetCurrentTime(initialTime);
        _currentPoints = 0;

        ActualizarScoreTxt();
        ActualizarTimeTxt();
    }

    public void SetIsInGame(bool changeBool)
    {
        _inGame = changeBool;
    }

}