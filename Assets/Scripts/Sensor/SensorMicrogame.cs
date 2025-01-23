using UnityEngine;

public class SensorMicrogame : MonoBehaviour
{
    [SerializeField] private int SetMicrogameId = 0; // ID del microjuego asociado
    private Renderer _sensorRenderer;
    private AppearGameScreen _appearGameScreen = null;

    [Header("COLORS")]
    [SerializeField] private Color CompletedColor = Color.white;
    [SerializeField] private Color NotCompletedColor = Color.white;
    private void Start()
    {
        _sensorRenderer = GetComponent<Renderer>(); // Asegura que se asigna el Renderer si no se ha asignado

        _appearGameScreen = FindObjectOfType<AppearGameScreen>();
        Invoke("CheckGameStatus", 0.5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_appearGameScreen != null && other.CompareTag("Player"))
        {
            _appearGameScreen.SetCurrentGame(SetMicrogameId);
            CheckGameStatus();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_appearGameScreen != null && other.CompareTag("Player"))
        {
            _appearGameScreen.SetCurrentGame(0);
        }
    }

    public void SetSensorColor(Color color)
    {
        if (_sensorRenderer != null)
        {
            _sensorRenderer.material.color = color;
        }
    }

    public int GetSetMicrogameId() => SetMicrogameId;

    private void CheckGameStatus()
    {
        // Cambia de color verde si ya está completado
        if (_appearGameScreen.IsGameWon(SetMicrogameId))
        {
            SetSensorColor(CompletedColor);
        }
        else
        {
            SetSensorColor(NotCompletedColor);
        }
    }
}
