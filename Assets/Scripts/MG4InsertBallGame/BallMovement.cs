using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody _rigidbody = null;
    [SerializeField] private float Ydestroy = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
#if UNITY_STANDALONE
        float directionX = Input.GetAxis("Horizontal");
        float directionZ = Input.GetAxis("Vertical");
#else
        float directionX = Input.acceleration.x * 2;
        float directionZ = Input.acceleration.y * 2;
#endif
        Vector3 movement = new Vector3(directionX, 0, directionZ);
        _rigidbody.AddForce(movement / 10, ForceMode.VelocityChange);

        // Destruye la bola cuando llega hasta cierto punto en Y
        if (this.transform.position.y <= Ydestroy) Destroy(this.gameObject);
    }
}
