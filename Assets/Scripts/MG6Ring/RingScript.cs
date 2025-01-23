using UnityEngine;

public class RingScript : MonoBehaviour
{
    [SerializeField] private RingGameManager RingGameManager;

    [Space(10)]
    [Header("Spawn Position")]
    [SerializeField] private Vector2 MinMaxX = Vector2.zero;
    [SerializeField] private Vector2 MinMaxZ = Vector2.zero;
    [SerializeField] private float SpawnPositionY = 0;

    [Space(10)]
    [Header("Move")]
    [SerializeField] private float MoveSpeed = 2f;
    [SerializeField] private float FallDistance = 0;
    private bool _movingDown = true;
    private Vector3 _initialPosition = Vector3.zero;
    private Vector3 _targetPosition = Vector3.zero;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(_initialPosition, _targetPosition);

    }
    private void OnEnable()
    {
        SetRandomPosition();
    }

    private void Update()
    {
        // Alterna entre movimiento hacia abajo y hacia arriba
        if (_movingDown)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * MoveSpeed);

            // Cambia de direccion al alcanzar la posicion baja
            if (Vector3.Distance(transform.position, _targetPosition) < 0.1f)
            {
                _movingDown = false;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _initialPosition, Time.deltaTime * MoveSpeed);

            // Cambia de direccion al alcanzar la posicion de inicio
            if (Vector3.Distance(transform.position, _initialPosition) < 0.1f)
            {
                _movingDown = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RingGameManager.AddPoint();
            SetRandomPosition();
        }
    }



    private void SetRandomPosition() 
    {
        float SpawnPositionX = Random.Range(MinMaxX.x, MinMaxX.y);
        float SpawnPositionZ = Random.Range(MinMaxZ.x, MinMaxZ.y);

        this.transform.position = new Vector3(SpawnPositionX, SpawnPositionY, SpawnPositionZ);

        _initialPosition = this.transform.position;
        _targetPosition = new Vector3(_initialPosition.x, _initialPosition.y - FallDistance, _initialPosition.z);
    }
}
