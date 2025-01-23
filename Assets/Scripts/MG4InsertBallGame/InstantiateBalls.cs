using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BallMaterial
{
    public Material Material;
    public string Tag;
}

public class InstantiateBalls : MonoBehaviour
{
    [SerializeField] private GameObject BallPrefab = null;
    [SerializeField] private BallMaterial[] BallMaterial = null;
    [SerializeField] private int NumberOfBalls = 6;

    [Space(10)]
    [Header("Instanciar bolas")]
    [SerializeField] private float InstanciaY = 0;

    private List<GameObject> instantiatedBalls = new List<GameObject>();

    private void OnEnable()
    {
        CreateBalls();
    }

    private void OnDisable()
    {
        EliminateBalls();
    }

    public void CreateBalls()
    {
        int halfBalls = NumberOfBalls / 2;

        // Instancia las bolas con su respectivo material y tag
        for (int i = 0; i < NumberOfBalls; i++)
        {
            Vector3 spawnPosition = GenerateRandomPosition(-10, 10, -5, 5);
            GameObject ball = Instantiate(BallPrefab, spawnPosition, Quaternion.identity);

            BallMaterial assignedMaterial = i < halfBalls ? BallMaterial[0] : BallMaterial[1];
            Renderer ballRenderer = ball.GetComponent<Renderer>();
            ballRenderer.material = assignedMaterial.Material;
            ball.tag = assignedMaterial.Tag;

            instantiatedBalls.Add(ball);
        }
    }

    public void EliminateBalls() 
    {
        // Destruye todas las bolas instanciadas
        foreach (GameObject ball in instantiatedBalls)
        {
            if (ball != null)
            {
                Destroy(ball);
            }
        }

        instantiatedBalls.Clear();
    }

    // Genera una posición aleatoria, evitando el centro
    private Vector3 GenerateRandomPosition(float min, float max, float innerMin, float innerMax)
    {
        float x, z;

        do
        {
            x = Random.Range(min, max);
            z = Random.Range(min, max);
        }
        while (x > innerMin && x < innerMax && z > innerMin && z < innerMax);

        return new Vector3(x, InstanciaY, z);
    }
}
