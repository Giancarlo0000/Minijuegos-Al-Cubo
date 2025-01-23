using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed = 20f; // Velocidad de la bala
    [SerializeField] private float lifetime = 5f; // Tiempo de vida antes de destruirse

    private Vector3 direction; // Direcci�n calculada al momento del disparo

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destruir la bala tras el tiempo de vida
    }

    private void Update()
    {
        // Mover la bala en la direcci�n establecida
        transform.position += direction * speed * Time.deltaTime;
    }

    public void SetTarget(Vector3 target)
    {
        // Calcular direcci�n hacia el objetivo
        direction = (target - transform.position).normalized;
    }
}
