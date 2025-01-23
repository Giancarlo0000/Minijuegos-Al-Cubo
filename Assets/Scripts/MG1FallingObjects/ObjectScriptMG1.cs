using UnityEngine;

public class ObjectScriptMG1 : MonoBehaviour
{
    private enum ObjectTypes{MORETIME, DAMAGE, POINT}

    [Header("Parameter Values")]
    [SerializeField] private float destroyObjectWhenYisIn = -5f;
    [SerializeField] private float velocityFalling = 14;

    [Space(10)]
    [Header("Select the type of object")]
    [SerializeField] private ObjectTypes objectTypes = default;

    [Space(10)]
    [Header ("If is MORETIME")]
    [SerializeField] private int addMoreTime = 5;

    private void Update()
    {
        // Destruye el objeto si su posición en Y es menor que destroyObjectWhenYisIn
        if (this.transform.position.y <= destroyObjectWhenYisIn) Destroy(this.gameObject);

        // El objeto "cae"
        GetComponent<Rigidbody>().velocity = new Vector2(0, -velocityFalling);
    }

    private void OnDisable()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectTypes == ObjectTypes.MORETIME) 
            {
                // Agrega más tiempo de acuerdo a addMoreTime
                FindObjectOfType<DataGameMG1>().AddMoreTime(addMoreTime);
            }

            if (objectTypes == ObjectTypes.DAMAGE)
            {
                // Sale de la ventana del microjuego
                FindObjectOfType<DataGameMG1>().Lose();
            }

            if (objectTypes == ObjectTypes.POINT)
            {
                // Agrega un punto
                FindObjectOfType<DataGameMG1>().AddScore();
            }

            Destroy(this.gameObject);
        }
    }
}
