using UnityEngine;
using UnityEngine.UI;

public class DataGameMG1 : MonoBehaviour
{
    private AppearGameScreen _appearGameScreen = null;
    private bool _inGame = false;
    private bool _canStart = false;

    [Header("SELECT")]
    [SerializeField] private GameObject FallingObjectMG1 = null;
    [SerializeField] private Button ActionButton = null;
    [SerializeField] private Button CloseButton = null;

    [Space(10)]
    [Header("Time")]
    [SerializeField] private Text timeTxt = null;
    [SerializeField] private int initialTime;
    private float _currentTime = 0;
    private float _timeCounter = 1f;

    [Space(10)]
    [Header("Score")]
    [SerializeField] private Text scoreTxt = null;
    [SerializeField] private int winningScore = 3;
    private int _score = 0;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private AudioClip addScoreSFX = null;
    [SerializeField] private AudioClip wrongSFX = null;
    [SerializeField] private AudioClip moreTimeSFX = null;

    void Start()
    {
        SetCurrentTime(initialTime);
        _appearGameScreen = FindObjectOfType<AppearGameScreen>();

        CloseButton.onClick.AddListener(() => CloseGameWindow());
    }

    void Update()
    {
        if (_score >= winningScore)
        {
            _appearGameScreen.MarkGameAsWon(1);
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
                Lose();
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
        if (other.CompareTag("Player") && !_appearGameScreen.IsGameWon(1))
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

    public void AddScore()
    {
        _score++;
        playerData.PlaySoundEffect(addScoreSFX);

        ActualizarScoreTxt();
    }

    public void AddMoreTime(int timeAdded)
    {
        playerData.PlaySoundEffect(moreTimeSFX);

        _currentTime += timeAdded;
        ActualizarTimeTxt();
    }

    public void Lose() 
    {
        CloseGameWindow();
        playerData.PlaySoundEffect(wrongSFX);
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
        FallingObjectMG1.SetActive(isActive);
        timeTxt.gameObject.SetActive(isActive);
        scoreTxt.gameObject.SetActive(isActive);
    }

    private void ActualizarScoreTxt() => scoreTxt.text = $"Score: {_score} / {winningScore}";
    private void ActualizarTimeTxt() => timeTxt.text = $"Tiempo restante: {_currentTime}";
    private void SetCurrentTime(float time) => _currentTime = time;

    private void ReiniciarJuego()
    {
        SetCurrentTime(initialTime);
        _score = 0;

        ActualizarScoreTxt();
        ActualizarTimeTxt();
    }

    public void SetIsInGame(bool changeBool)
    {
        _inGame = changeBool;
    }

    public bool GetIsInGame() => _inGame;
}