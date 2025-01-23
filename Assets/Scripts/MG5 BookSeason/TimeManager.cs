using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimeManager : MonoBehaviour
{
    [Header("Time Settings")]
    [SerializeField] private float startTime = 30; // Tiempo inicial en segundos

    [Header("UI Reference")]
    [SerializeField] private Text timerText; // Texto del Canvas para mostrar el tiempo restante

    [Header("SFX")]
    [SerializeField] private AudioClip resetSFX = null;
    private bool isRestarting = false;
    private AudioSource _audioSoruce = null;

    private float currentTime; // Tiempo actual

    private void Start()
    {
        isRestarting = false;
        _audioSoruce = GetComponent<AudioSource>();

        // Inicializa el tiempo actual
        currentTime = startTime;

        // Actualiza el texto del timer al inicio
        UpdateTimerText();
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
        }

        // Verifica si el tiempo llegó a 0
        if (currentTime <= 0 && !isRestarting)
        {
            currentTime = 0;
            isRestarting = true;
            StartCoroutine(RestartLevel());
        }

        // Actualiza el texto del timer
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = $"Tiempo restante: {seconds:00}";
    }

    public void PlaySFX(AudioClip sfx) 
    {
        _audioSoruce.clip = sfx;
        _audioSoruce.Play();
    }

    private IEnumerator RestartLevel()
    {
        PlaySFX(resetSFX);

        // Espera el tiempo de duración del audio
        yield return new WaitForSeconds(resetSFX.length);

        // Recarga la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
