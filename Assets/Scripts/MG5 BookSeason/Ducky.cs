using System.Collections.Generic;
using UnityEngine;

public class Ducky : MonoBehaviour
{
    public enum DuckyState { Verde, Amarillo, Azul, Rojo } // Estados disponibles

    [System.Serializable]
    public class StateMaterial
    {
        public DuckyState state; // Estado del Ducky
        public Material material; // Material asociado
    }

    [Header("Estados y colores")]
    [SerializeField] private List<StateMaterial> stateMaterials; // Lista de estados y materiales

    [Header("Configuración")]
    [SerializeField] private Vector2 stateChangeTimeRange = new Vector2(1f, 3f); // Rango de tiempo para cambio de estado

    private Renderer duckyRenderer; // Renderer para cambiar el material
    private DuckyState currentState; // Estado actual del Ducky

    private Dictionary<DuckyState, Material> stateToMaterial; // Mapeo de estados a materiales

    public Transform targetDestination; // Destino actual del objeto

    [Header("Velocidad")]
    [SerializeField] private Vector2 speedRange = new Vector2(3f, 6f); // Rango de velocidad (mínimo y máximo)

    private float speed; // Velocidad seleccionada aleatoriamente
    private float nextStateChangeTime; // Tiempo para el próximo cambio de estado

    private Vector3 randomRotation; // Vector para la rotación aleatoria

    private void Start()
    {
        // Validar que targetDestination está asignado
        if (targetDestination == null)
        {
            Debug.LogError("Target Destination no está asignado. Por favor, asigna un Transform válido.");
            return;
        }

        // Inicializar velocidad con un valor aleatorio entre el rango definido
        speed = Random.Range(speedRange.x, speedRange.y);

        // Inicializar Renderer y materiales
        duckyRenderer = GetComponent<Renderer>();
        InitializeMaterials();

        // Generar una rotación aleatoria
        randomRotation = new Vector3(
            Random.Range(-90f, 90f),
            Random.Range(-90f, 90f),
            Random.Range(-90f, 90f)
        );

        // Configurar el tiempo inicial para cambio de estado
        ScheduleNextStateChange();
    }

    private void InitializeMaterials()
    {
        // Crear el diccionario de estados y materiales
        stateToMaterial = new Dictionary<DuckyState, Material>();

        foreach (var stateMaterial in stateMaterials)
        {
            if (!stateToMaterial.ContainsKey(stateMaterial.state))
            {
                stateToMaterial[stateMaterial.state] = stateMaterial.material;
            }
        }

        // Establecer el estado inicial
        SetState(DuckyState.Verde);
    }

    private void ScheduleNextStateChange()
    {
        // Determinar un tiempo aleatorio dentro del rango definido
        float changeInterval = Random.Range(stateChangeTimeRange.x, stateChangeTimeRange.y);
        nextStateChangeTime = Time.time + changeInterval;
    }

    private void Update()
    {
        if (targetDestination != null)
        {
            // Mover el objeto hacia el destino
            transform.position = Vector3.MoveTowards(transform.position, targetDestination.position, speed * Time.deltaTime);

            // Aplicar rotación aleatoria en X, Y y Z
            transform.Rotate(randomRotation * Time.deltaTime);

            // Verificar si llegó al destino
            if (Vector3.Distance(transform.position, targetDestination.position) < 0.1f)
            {
                Destroy(gameObject); // Destruir si llega al destino (sin contar para la lista)
            }
        }

        // Cambiar estado si ha pasado el tiempo
        if (Time.time >= nextStateChangeTime)
        {
            ChangeState();
            ScheduleNextStateChange();
        }
    }

    private void ChangeState()
    {
        // Cambiar al siguiente estado
        currentState = (DuckyState)(((int)currentState + 1) % System.Enum.GetValues(typeof(DuckyState)).Length);
        SetState(currentState);
    }

    private void SetState(DuckyState state)
    {
        currentState = state;

        // Cambiar el material según el estado actual
        if (stateToMaterial.ContainsKey(state) && duckyRenderer != null)
        {
            duckyRenderer.material = stateToMaterial[state];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Mostrar en consola el estado actual
            Debug.Log($"Cazaste a un Ducky de color {currentState}");

            // Notificar al ColorListManager pasando el estado como string
            FindObjectOfType<ColorListManager>()?.CheckDuckyColor(currentState.ToString());

            Destroy(gameObject); // Destruir este objeto (Ducky)
            Destroy(other.gameObject); // Destruir la bala también
        }
    }
}
