using UnityEngine;
using UnityEngine.UI;

public class MicrojuegoPresionaEnOrden : MonoBehaviour
{
    public Text temporizadorTexto;  // Referencia al texto del temporizador
    public Button[] botones;        // Array con los botones numerados
    public float tiempoLimite = 10f; // Tiempo límite para completar el juego

    private int siguienteNumero = 1; // Número que el jugador debe presionar
    private float tiempoRestante;
    private bool _inGame = false;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private AudioClip loseSFX = null;
    [SerializeField] private AudioClip rightButtonSFX = null;
    [SerializeField] private AudioClip winSFX = null;
    private PlayerData _playerData = null;

    void OnEnable()
    {
        _playerData = FindAnyObjectByType<PlayerData>();
        IniciarMicrojuego();
    }

    void Update()
    {
        if (_inGame) 
        {
            if (tiempoRestante > 0)
            {
                tiempoRestante -= Time.deltaTime;
                temporizadorTexto.text = "Tiempo restante: " + Mathf.Ceil(tiempoRestante).ToString();

                if (tiempoRestante <= 0)
                {
                    FinDelJuego(false);
                }
            }
        }
    }

    void IniciarMicrojuego()
    {
        // Reinicia los valores al iniciar el microjuego
        siguienteNumero = 1;
        tiempoRestante = tiempoLimite;

        // Genera una secuencia de números aleatoria
        int[] numerosAleatorios = GenerarSecuenciaAleatoria(botones.Length);

        // Habilita todos los botones, asigna los números aleatorios y los eventos
        for (int i = 0; i < botones.Length; i++)
        {
            Button botonActual = botones[i]; // Captura el botón en una variable local
            botonActual.interactable = true;
            botonActual.GetComponentInChildren<Text>().text = numerosAleatorios[i].ToString();
            botonActual.onClick.RemoveAllListeners(); // Elimina eventos previos
            botonActual.onClick.AddListener(() => VerificarBoton(botonActual)); // Usa la variable local
        }
    }


    // Método para generar una secuencia aleatoria
    int[] GenerarSecuenciaAleatoria(int cantidad)
    {
        int[] numeros = new int[cantidad];
        for (int i = 0; i < cantidad; i++)
        {
            numeros[i] = i + 1;
        }

        // Mezcla los números utilizando Fisher-Yates Shuffle
        for (int i = numeros.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = numeros[i];
            numeros[i] = numeros[randomIndex];
            numeros[randomIndex] = temp;
        }

        return numeros;
    }


    void VerificarBoton(Button botonPresionado)
    {
        // Verifica si el botón tiene el número correcto.
        if (botonPresionado.GetComponentInChildren<Text>().text == siguienteNumero.ToString())
        {
            _playerData.PlaySoundEffect(rightButtonSFX);

            botonPresionado.interactable = false; // Desactiva el botón
            siguienteNumero++;

            // Comprueba si todos los botones fueron presionados
            if (siguienteNumero > botones.Length)
            {
                FinDelJuego(true);
            }
        }
        else
        {
            FinDelJuego(false); // Fin del juego si se presiona un botón incorrecto
        }
    }

    void FinDelJuego(bool gano)
    {
        tiempoRestante = 0; // Detiene el temporizador

        // Desactiva todos los botones.
        foreach (Button boton in botones)
        {
            boton.interactable = false;
        }

        if (gano)
        {
            // Marca el microjuego como ganado
            FindObjectOfType<AppearGameScreen>().MarkGameAsWon(2);
            _playerData.PlaySoundEffect(winSFX);
        }

        else _playerData.PlaySoundEffect(loseSFX);

        // Cierra el panel del microjuego
        FindObjectOfType<AppearGameScreen>().CloseMicrogameWindow();
    }


    public void SetInGame(bool isornot) => _inGame = isornot;
}
