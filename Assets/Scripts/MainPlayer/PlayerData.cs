using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    private int _totalGamesWon = 0;
    private int _currentGamesWon = 0;

    [Header("Texto en la escena")]
    [SerializeField] private Text currentGamesWonText = null;

    [Space(10)]
    [Header("AppearGameScreen")]
    [SerializeField] private AppearGameScreen _appearGameScreen;

    private AudioSource _audioSource = null;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        // Buscar el componente AppearGameScreen en la misma escena.
        _appearGameScreen = GetComponent<AppearGameScreen>();

        // Inicializar el texto con el número actual de partidas ganadas.
        UpdateGamesWonText();
    }

    public void UpdateGamesWonText()
    {
        if (_appearGameScreen != null)
        {
            int completedGames = _appearGameScreen.GetCompletedGamesCount();
            int totalGames = _appearGameScreen.GetTotalGamesCount();

            // Actualiza el texto en pantalla.
            currentGamesWonText.text = $"{completedGames}/{totalGames}";
        }
        else
        {
            Debug.LogError("No hay seleccionado un AppearGameScreen");
        }
    }

    public void PlaySoundEffect(AudioClip audioClipPlay) 
    {
        _audioSource.clip = audioClipPlay;
        _audioSource.Play();
    }
}
