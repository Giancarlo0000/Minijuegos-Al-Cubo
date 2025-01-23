using UnityEngine;

public class PlayerMoveMG1 : MonoBehaviour
{
    private Rigidbody _rigidbody = null;
    [SerializeField] private float PlayerVelocity = 7f;
    private Vector3 _initialPosition = Vector3.zero;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _initialPosition = this.transform.position;
    }

    private void Update()
    {
#if UNITY_STANDALONE
        float directionX = Input.GetAxis("Horizontal");
#else
        float directionX = Input.acceleration.x;
#endif
        Vector3 movement = new Vector3(directionX, 0, 0);
        _rigidbody.velocity = movement * PlayerVelocity;
    }

    private void OnDisable()
    {
        this.transform.position = _initialPosition;
    }
}
