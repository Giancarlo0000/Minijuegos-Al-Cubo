using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableObject
    {
        public GameObject prefab;           // El prefab que quieres spawnear
        public Vector2 spawnTimeRange;      // Rango de tiempo (mínimo y máximo) para spawnear
        public int maxSpawnLimit;           // Límite máximo de este objeto en escena por este spawner
    }

    [System.Serializable]
    public class SpawnerTarget
    {
        public Transform spawner; // El transform del spawner de origen
        public Transform target;  // El transform del objetivo al que irá el objeto
    }

    public List<SpawnerTarget> spawnerTargets = new List<SpawnerTarget>();
    public List<SpawnableObject> spawnableObjects = new List<SpawnableObject>();

    private Dictionary<GameObject, int> spawnedCount = new Dictionary<GameObject, int>(); // Cuenta de objetos spawneados

    private void Start()
    {
        foreach (var spawnableObject in spawnableObjects)
        {
            spawnedCount[spawnableObject.prefab] = 0; // Inicializar conteo en 0 para cada prefab
            StartCoroutine(SpawnRoutine(spawnableObject));
        }
    }

    private IEnumerator SpawnRoutine(SpawnableObject spawnableObject)
    {
        while (true)
        {
            float delay = Random.Range(spawnableObject.spawnTimeRange.x, spawnableObject.spawnTimeRange.y);
            yield return new WaitForSeconds(delay);

            // Verificar si se ha alcanzado el límite
            if (spawnedCount[spawnableObject.prefab] < spawnableObject.maxSpawnLimit)
            {
                if (spawnableObject.prefab != null && spawnerTargets.Count > 0)
                {
                    // Elegir un spawner al azar
                    SpawnerTarget selectedSpawnerTarget = spawnerTargets[Random.Range(0, spawnerTargets.Count)];

                    // Instanciar objeto en el spawner seleccionado
                    GameObject spawnedObject = Instantiate(
                        spawnableObject.prefab,
                        selectedSpawnerTarget.spawner.position,
                        selectedSpawnerTarget.spawner.rotation
                    );

                    // Configurar el destino en el Ducky
                    Ducky ducky = spawnedObject.GetComponent<Ducky>();
                    if (ducky != null)
                    {
                        ducky.targetDestination = selectedSpawnerTarget.target;
                    }

                    // Aumentar contador de objetos spawneados
                    spawnedCount[spawnableObject.prefab]++;

                    // Registrar destrucción del objeto para disminuir el contador
                    spawnedObject.AddComponent<SpawnedObjectTracker>().Setup(spawnableObject.prefab, this);
                }
            }
        }
    }

    public void DecreaseSpawnedCount(GameObject prefab)
    {
        if (spawnedCount.ContainsKey(prefab))
        {
            spawnedCount[prefab] = Mathf.Max(0, spawnedCount[prefab] - 1); // Reducir el contador asegurando que no sea negativo
        }
    }
}

public class SpawnedObjectTracker : MonoBehaviour
{
    private GameObject trackedPrefab;
    private Spawn spawner;

    public void Setup(GameObject prefab, Spawn spawner)
    {
        this.trackedPrefab = prefab;
        this.spawner = spawner;
    }

    private void OnDestroy()
    {
        // Notificar al spawner que un objeto ha sido destruido
        if (spawner != null && trackedPrefab != null)
        {
            spawner.DecreaseSpawnedCount(trackedPrefab);
        }
    }
}
