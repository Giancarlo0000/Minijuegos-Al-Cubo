using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Sniper : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform crosshair; // Referencia a la imagen de la mira
    [SerializeField] private Canvas canvas; // Canvas de la mira
    [SerializeField] private Transform sniperMan; // GameObject Sniper Man
    [SerializeField] private Transform rumbo; // GameObject Rumbo
    [SerializeField] private GameObject bulletPrefab; // Prefab de la bala
    [SerializeField] private Image reloadIndicator; // Imagen del indicador de recarga

    [Header("Settings")]
    [SerializeField] private float reloadTime = 1.5f; // Tiempo de recarga en segundos
    [SerializeField] private Color reloadColor = new Color(1, 1, 1, 0.5f); // Color cuando está recargando
    [SerializeField] private Color readyColor = Color.white; // Color cuando está listo para disparar
    [SerializeField] private float sniperManZ = -15f; // Z inicial de Sniper Man
    [SerializeField] private float rumboStartZ = -10f; // Z inicial de Rumbo
    [SerializeField] private float sensitivity = 1f; // Sensibilidad del movimiento
    [SerializeField] private Joystick Joystick = null; // Joystick de celular
    [SerializeField] private Button ShootButton = null; // Joystick de celular
    [SerializeField] private float AimSpeed = 100f; // Joystick de celular

    [Header("SFX")]
    [SerializeField] private TimeManager timeManager = null;
    [SerializeField] private AudioClip shootSFX = null;



    private Camera mainCamera;
    private bool isReloading = false; // Controla si el arma está recargando
    private Vector3 _direction = Vector3.zero;

    private void Start()
    {
#if UNITY_STANDALONE
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
#endif

        mainCamera = Camera.main;   

#if UNITY_STANDALONE
Joystick.gameObject.SetActive(false);
        ShootButton.gameObject.SetActive(false);
#else
        Joystick.gameObject.SetActive(true);
        ShootButton.gameObject.SetActive(true);
        ShootButton.onClick.AddListener(Shoot);

#endif

        // Configurar posiciones iniciales
        if (sniperMan != null)
        {
            Vector3 initialSniperPosition = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Mathf.Abs(sniperManZ)));
            sniperMan.position = new Vector3(initialSniperPosition.x, initialSniperPosition.y, sniperManZ);
        }

        if (rumbo != null)
        {
            Vector3 initialRumboPosition = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Mathf.Abs(rumboStartZ)));
            rumbo.position = new Vector3(initialRumboPosition.x, initialRumboPosition.y, rumboStartZ);
        }

        // Configurar el estado inicial del indicador de recarga
        if (reloadIndicator != null)
        {
            reloadIndicator.color = readyColor;
        }
    }

    private void MobileAim()
    {
        _direction.x = Joystick.Horizontal;
        _direction.y = Joystick.Vertical;
        crosshair.localPosition += AimSpeed * Time.deltaTime * _direction;
        rumbo.position += AimSpeed * Time.deltaTime * _direction / 85;
    }
    private void Update()
    {
#if UNITY_STANDALONE
        if (crosshair == null || canvas == null || rumbo == null)
        {
            Debug.LogError("Faltan referencias en el script Sniper.");
            return;
        }

        // Mover la mira y Rumbo en X, Y
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.GetComponent<RectTransform>(),
            mousePosition,
            canvas.worldCamera,
            out Vector2 localPoint
        );
        
        crosshair.localPosition = localPoint * sensitivity;

        Vector3 screenPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Mathf.Abs(rumboStartZ)));

        rumbo.position = new Vector3(screenPosition.x, screenPosition.y, rumboStartZ);
        
        // Detectar clic izquierdo del mouse para disparar
        if (Input.GetMouseButtonDown(0) && bulletPrefab != null)
        {
            Shoot();
        }
#else
        MobileAim();
#endif
    }

    private void Shoot()
    {
        if (!isReloading) {
            if (sniperMan == null || rumbo == null) return;
            timeManager.PlaySFX(shootSFX);

            // Crear la bala y dirigirla hacia Rumbo
            GameObject bullet = Instantiate(bulletPrefab, sniperMan.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();

            if (bulletScript != null)
            {
                bulletScript.SetTarget(rumbo.position);
            }

            // Iniciar el proceso de recarga
            StartCoroutine(Reload());
        }
       
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        // Cambiar el color de la imagen de recarga
        if (reloadIndicator != null)
        {
            reloadIndicator.color = reloadColor;
        }

        // Esperar el tiempo de recarga
        yield return new WaitForSeconds(reloadTime);

        isReloading = false;

        // Restaurar el color de la imagen de recarga
        if (reloadIndicator != null)
        {
            reloadIndicator.color = readyColor;
        }
    }

    private void OnDisable()
    {
        // Restaurar el cursor cuando se desactive el objeto
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
