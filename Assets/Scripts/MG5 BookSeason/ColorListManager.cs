using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ColorListManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Canvas parentCanvas; // Canvas donde están las imágenes
    [SerializeField] private List<Image> colorImages; // Lista de imágenes en el Canvas

    [Header("Color Settings")]
    [SerializeField] private List<ColorState> possibleColors; // Lista de colores posibles (hexadecimal + estado) 

    [Header("Game Settings")]
    [SerializeField] private string MainGameplaySceneName = string.Empty;

    [Header("SFX")]
    [SerializeField] private AudioClip goodSFX = null;
    [SerializeField] private AudioClip wrongSFX = null;
    private TimeManager timeManager = null;

    private List<ColorState> targetColors; // Lista generada aleatoriamente
    private int currentIndex = 0; // Índice del color actual en la lista

    [System.Serializable]
    public class ColorState
    {
        public string colorHex;
        public string stateName;
    }

    private void Start()
    {
        timeManager = FindAnyObjectByType<TimeManager>();

        // Genera y asigna colores
        AssignRandomColorsToImages();
    }

    private void AssignRandomColorsToImages()
    {
        // Inicializa la lista de colores objetivo
        targetColors = new List<ColorState>();

        for (int i = 0; i < colorImages.Count; i++)
        {
            // Obtiene un color aleatorio de la lista de colores posibles
            ColorState randomColor = GetRandomColor();
            targetColors.Add(randomColor);

            // Convierte de hexadecimal a color
            Color color;
            if (ColorUtility.TryParseHtmlString("#" + randomColor.colorHex, out color))
            {
                colorImages[i].color = color;
            }
        }
    }

    private ColorState GetRandomColor()
    {
        // Selecciona un color aleatorio de la lista de posibles colores
        int randomIndex = Random.Range(0, possibleColors.Count);
        return possibleColors[randomIndex];
    }

    public void CheckDuckyColor(string duckyColorName)
    {
        // Verifica si el color del Ducky coincide con el color actual
        if (currentIndex < targetColors.Count && duckyColorName == targetColors[currentIndex].stateName)
        {
            timeManager.PlaySFX(goodSFX);
            Advance();
        }
        else
        {
            timeManager.PlaySFX(wrongSFX);
            Regress();
        }
    }

    private void Advance()
    {
        // Desactiva la imagen actual
        colorImages[currentIndex].gameObject.SetActive(false);

        // Avanza al siguiente si no es el último
        if (currentIndex < targetColors.Count - 1)
        {
            currentIndex++;
        }
        else
        {
            FindObjectOfType<GlobalGameData>().SetIsCompleted(4, true);
            ReturnToMainGameplay();
        }
    }

    private void Regress()
    {
        // Reactiva la imagen anterior si no estamos en el primero
        if (currentIndex > 0)
        {
            currentIndex--;
            colorImages[currentIndex].gameObject.SetActive(true);
        }
    }

    public void ReturnToMainGameplay()
    {
        SceneManager.LoadScene(MainGameplaySceneName);
    }
}
