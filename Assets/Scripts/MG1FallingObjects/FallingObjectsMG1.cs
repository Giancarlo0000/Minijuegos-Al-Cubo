using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectFalling
{
    public GameObject objectFalling;
    [Range(0, 100)] public int percentage;
}

public class FallingObjectsMG1 : MonoBehaviour
{
    [Header("OBJECTS")]
    [SerializeField] private List<ObjectFalling> objectsFalling; // Lista de objetos y porcentajes

    [Space(10)]
    [Header("TIMER OBJECT")]
    [SerializeField] private float appearObjectIn = 2f; // Tiempo entre apariciones
    private float _timer = 0f;

    [Space(10)]
    [Header("POSITION")]
    [SerializeField] private Vector3 minPosition; // Límite inferior de posición
    [SerializeField] private Vector3 maxPosition; // Límite superior de posición

    [Space(10)]
    [Header("CONTAINER")]
    [SerializeField] private Transform container;
    [SerializeField] private DataGameMG1 dataGameMG1 = null;

    void Update()
    {
        if (dataGameMG1.GetIsInGame()) 
        {
            _timer += Time.deltaTime;

            if (_timer >= appearObjectIn)
            {
                SpawnObject();  // Llamar para generar un objeto aleatorio
                _timer = 0f;
            }
        }
    }

    private void SpawnObject()
    {
        // Selecciona un objeto del pool basado en el porcentaje
        GameObject selectedObject = GetRandomObjectByPercentage();
        if (selectedObject == null) return;

        // Instanciar un nuevo objeto en la posición aleatoria
        Vector3 randomPosition = new Vector3(
            Random.Range(minPosition.x, maxPosition.x),
            Random.Range(minPosition.y, maxPosition.y),
            Random.Range(minPosition.z, maxPosition.z)
        );

        GameObject spawnedObject = Instantiate(selectedObject, randomPosition, Quaternion.identity);  // Instanciar objeto
        spawnedObject.transform.SetParent(container);
    }

    private GameObject GetRandomObjectByPercentage()
    {
        int totalPercentage = 0;

        // Sumar los porcentajes para normalizar
        foreach (var obj in objectsFalling)
        {
            totalPercentage += obj.percentage;
        }

        // Genera un valor aleatorio basado en el total de porcentajes
        int randomValue = Random.Range(0, totalPercentage);

        int cumulativePercentage = 0;
        // Selecciona el objeto basado en el porcentaje acumulado
        foreach (var obj in objectsFalling)
        {
            cumulativePercentage += obj.percentage;
            if (randomValue < cumulativePercentage)
            {
                return obj.objectFalling;
            }
        }

        return null; // En caso de que no se seleccione ningún objeto
    }
}
