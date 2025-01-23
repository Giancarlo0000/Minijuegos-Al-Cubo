using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearGameScreen : MonoBehaviour
{
    [SerializeField] private int _currentGameSelected = 0;
    [SerializeField] private Canvas MicrogameWindow = null;
    [SerializeField] private Transform MicrogameContentArea = null; // Zona donde se cargarán los microjuegos.
    [SerializeField] public List<GameObject> MicrogamePrefabs; // Lista de prefabs de microjuegos.
    private bool _isPlaying = false;

    [SerializeField] private Button ActionButton = null;
    [SerializeField] private Button JumpButton = null;
    [SerializeField] private Canvas MovementCanvas = null;

    [SerializeField] private GameObject ParticlePrefabWinGame = null;

    private GameObject _currentMicrogameInstance = null;
    private GlobalGameData _globalGameData = null;

    [SerializeField] private GameObject StandaloneImage = null;
    [SerializeField] private GameObject MobileImage = null;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private AudioClip winGameSfx = null;
    private PlayerData _playerData = null;


    private void Awake()
    {
        _playerData = GetComponent<PlayerData>();
        _globalGameData = FindObjectOfType<GlobalGameData>();
    }
    void Start()
    {
        // Desactiva las imágenes indicaciones
        StandaloneImage.SetActive(false);
        MobileImage.SetActive(false);

        // Desactiva la ventana de juego al iniciar
        MicrogameWindow.gameObject.SetActive(false);

#if UNITY_STANDALONE
        ActionButton.gameObject.SetActive(false);
        JumpButton.gameObject.SetActive(false);
#else
        ActionButton.gameObject.SetActive(true);
        JumpButton.gameObject.SetActive(true);
#endif
    }

    void Update()
    {
        if (_currentGameSelected != 0)
        {
#if UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.E)) StartMicrogame();
#endif

            if (!IsGameWon(_currentGameSelected))
            {
#if UNITY_STANDALONE
                StandaloneImage.SetActive(true);
#else
                MobileImage.SetActive(true);
#endif
            }
            else
            {
                // Desactiva las imágenes si el juego ya fue ganado
                StandaloneImage.SetActive(false);
                MobileImage.SetActive(false);
            }
        }
        else
        {
            // Desactiva las imágenes si no hay ningún microjuego seleccionado
            StandaloneImage.SetActive(false);
            MobileImage.SetActive(false);
        }
    }


    public void SetCurrentGame(int currentGameId) => _currentGameSelected = currentGameId;

    public int GetCurrentGame() => _currentGameSelected;

    public void CloseMicrogameWindow()
    {
        // Reactiva el movimiento del jugador
        GetComponent<MainPlayerMove>().SetCanMove(true);

        // Desactiva la ventana del microjuego
        MicrogameWindow.gameObject.SetActive(false);

        // Destruye el microjuego actual, si existe
        if (_currentMicrogameInstance != null)
        {
            Destroy(_currentMicrogameInstance);
        }

#if UNITY_STANDALONE
#else
        MovementCanvas.gameObject.SetActive(true);
#endif

        _isPlaying = false;
    }

    private void LoadMicrogame(int microgameId)
    {
        // Asegúrate de que no haya un microjuego activo antes de cargar uno nuevo
        if (_currentMicrogameInstance != null)
        {
            Destroy(_currentMicrogameInstance);
        }

        // Verifica si el ID es válido y carga el prefab correspondiente
        if (microgameId > 0 && microgameId <= MicrogamePrefabs.Count)
        {
            _currentMicrogameInstance = Instantiate(MicrogamePrefabs[microgameId - 1], MicrogameContentArea);
        }
        else
        {
            Debug.LogError("ID de microjuego inválido: " + microgameId);
        }

#if UNITY_STANDALONE
#else
        MovementCanvas.gameObject.SetActive(false);
#endif

        _isPlaying = true;
    }

    public void MarkGameAsWon(int gameId)
    {
        if (gameId > 0 && gameId <= _globalGameData._isMGCompleted.Count)
        {
            // SFX
            _playerData.PlaySoundEffect(winGameSfx);

            // Marca el microjuego como completado en la lista
            _globalGameData.SetIsCompleted(gameId - 1, true);

            GetComponent<PlayerData>().UpdateGamesWonText();

            //Agrega el particle system y lo elimina cuando termina el sistema
            GameObject particleInstance = Instantiate(ParticlePrefabWinGame, transform.position, Quaternion.Euler(-90, 0, 0), transform);

            ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(particleInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            }
        }
        else
        {
            Debug.LogError("ID de microjuego inválido: " + gameId);
        }
    }

    public bool IsGameWon(int gameId)
    {
        if (gameId > 0 && gameId <= _globalGameData._isMGCompleted.Count)
        {
            return _globalGameData._isMGCompleted[gameId - 1];
        }
        else
        {
            Debug.LogError("ID de microjuego inválido: " + gameId);
            return false; // Devuelve falso si el ID es inválido
        }
    }

    public bool AreAllGamesCompleted()
    {
        // Recorre la lista y verifica si todos los elementos son true
        foreach (bool status in _globalGameData._isMGCompleted)
        {
            if (!status)
            {
                return false; // Si encuentra un false, devuelve false
            }
        }
        return true; // Si no encontró ningún false, devuelve true
    }


    public void StartMicrogame()
    {
        if (!_isPlaying)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Verifica si no se ha seleccionado ningún microjuego
            if (_currentGameSelected == 0)
            {
                Debug.Log("No hay ningún microjuego seleccionado.");
                return; // Salir del método si no hay un microjuego seleccionado
            }

            // Verifica si el microjuego ya fue ganado
            if (_globalGameData._isMGCompleted[_currentGameSelected - 1])
            {
                Debug.Log("Este microjuego ya fue ganado.");
                return; // No permite abrir el microjuego si ya fue ganado
            }

            // Si el microjuego seleccionado es válido, proceder
            if (_currentGameSelected > 0)
            {
                // Desactiva el movimiento del jugador
                GetComponent<MainPlayerMove>().SetCanMove(false);

                // Abre la ventana del microjuego
                MicrogameWindow.gameObject.SetActive(true);

                // Cargar el microjuego correspondiente
                LoadMicrogame(_currentGameSelected);
            }
        }
    }

    public int GetCompletedGamesCount()
    {
        // Cuenta cuántos elementos en MicrogameCompletionStatus son verdaderos.
        int completedGames = 0;
        foreach (bool status in _globalGameData._isMGCompleted)
        {
            if (status) completedGames++;
        }
        return completedGames;
    }

    public int GetTotalGamesCount()
    {
        // Devuelve la cantidad total de microjuegos.
        return MicrogamePrefabs.Count;
    }
}
