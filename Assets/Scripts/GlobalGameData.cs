using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalGameData : MonoBehaviour
{
    private static GlobalGameData _instance;

    [SerializeField] private string PlayerObjectName = string.Empty;
    private Transform _playerTransform = null;
    [SerializeField] private Vector3 currentPlayerPosition = Vector3.zero;

    [SerializeField] private string[] scenesToDestroyScript = { "EndScreen", "MainMenu" };

    private AppearGameScreen _appearGameScreen;
    public List<bool> _isMGCompleted;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        _appearGameScreen = FindObjectOfType<AppearGameScreen>();

        _isMGCompleted = new List<bool>(new bool[_appearGameScreen.MicrogamePrefabs.Count]);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerObject();
        VerifySceneToDestroy();
    }

    private void FindPlayerObject()
    {
        GameObject playerObject = GameObject.Find(PlayerObjectName);

        if (playerObject != null)
        {
            // Se le asigna al player la última posición guardada
            playerObject.transform.position = currentPlayerPosition;

            // Se le asigna el transform al player
            _playerTransform = playerObject.transform;
            Debug.Log($"Objeto '{PlayerObjectName}' encontrado en la escena '{SceneManager.GetActiveScene().name}' y reposicionado.");
        }
        else
        {
            Debug.LogWarning($"Objeto '{PlayerObjectName}' no encontrado en la escena '{SceneManager.GetActiveScene().name}'.");
        }
    }

    private void Update()
    {
        // Actualiza la posición solo si el transform existe
        if (_playerTransform != null)
        {
            currentPlayerPosition = _playerTransform.position;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void VerifySceneToDestroy()
    {
        // Comprueba si la escena actual es una de las que debe destruir el script
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Destruye el GameObject que contiene el script
        foreach (string scene in scenesToDestroyScript)
        {
            if (currentSceneName == scene)
            {
                Destroy(gameObject);
                return;
            }
        }
    }

    public void SetIsCompleted(int idGame, bool isCompleted) => _isMGCompleted[idGame] = isCompleted;
    public bool GetIsCompleted(int idGame) => _isMGCompleted[idGame];
}
