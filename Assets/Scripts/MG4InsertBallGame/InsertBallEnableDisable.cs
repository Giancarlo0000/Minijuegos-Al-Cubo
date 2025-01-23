using UnityEngine;
using UnityEngine.UI;

public class InsertBallEnableDisable : MonoBehaviour
{
    [SerializeField] private GameObject InsertBallGameobject = null;
    [SerializeField] private Button CloseButton = null;
    [SerializeField] private Button ActionButton = null;
    private bool _canStart = false;

    private void Start()
    {
        // Asigna el método EnableGameobject al botón al iniciar
        CloseButton.onClick.AddListener(() => EnableGameobject(false));
    }

    private void Update()
    {
        if (_canStart == true && Input.GetKeyDown(KeyCode.E))
        {
            EnableGameobject(true);
            _canStart = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !FindObjectOfType<AppearGameScreen>().IsGameWon(4))
        {
            if (FindObjectOfType<AppearGameScreen>().GetCurrentGame() == 4)
            {
#if UNITY_STANDALONE
                _canStart = true;
#else 
                ActionButton.onClick.AddListener(() => EnableGameobject(true));
#endif
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canStart = false;
            ActionButton.onClick.RemoveAllListeners();
        }
    }

    public void EnableGameobject(bool isEnabled) 
    {
        InsertBallGameobject.SetActive(isEnabled);
    }
}
