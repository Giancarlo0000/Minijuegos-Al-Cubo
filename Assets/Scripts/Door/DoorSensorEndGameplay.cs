using System.Collections;
using UnityEngine;

public class DoorSensorEndGameplay : MonoBehaviour
{
    private AppearGameScreen _playerAppearGameScreen;

    [Header("Door")]
    [SerializeField] private GameObject DoorGameobject = null;
    private bool _completedAllGames = false;

    [Space(10)]
    [Header("Animation door CORRECT")]
    private Transform _doorTransform = null;
    private Vector3 _initialPosition;
    [SerializeField] private Vector3 ModificationPositionValue = Vector3.zero;
    [SerializeField] private float MoveDuration = 1f;

    [Space(10)]
    [Header("Animation door INCORRECT")]
    private Renderer _doorRenderer = null;
    private Color _originalColor = Color.white;
    [SerializeField] private Color ChangeColorTo = Color.white;
    [SerializeField] private float ChangeColorDuration = 1f;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private AudioClip doorCorrectSFX = null;
    [SerializeField] private AudioClip doorIncorrectSFX = null;

    private void Start()
    {
        _playerAppearGameScreen = FindObjectOfType<AppearGameScreen>();

        _doorRenderer = DoorGameobject.GetComponent<Renderer>();
        _originalColor = _doorRenderer.material.color;

        _doorTransform = DoorGameobject.GetComponent<Transform>();
        _initialPosition = _doorTransform.position;
    }

    private IEnumerator AnimationDoorCorrect()
    {
        _completedAllGames = true;

        float elapsedTime = 0f;
        Vector3 TargetPosition = _initialPosition + ModificationPositionValue;

        while (elapsedTime < MoveDuration)
        {
            _doorTransform.position = Vector3.Lerp(_initialPosition, TargetPosition, elapsedTime / MoveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _doorTransform.position = TargetPosition;

    }

    private IEnumerator AnimationDoorIncorrect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < ChangeColorDuration)
        {
            _doorRenderer.material.color = Color.Lerp(ChangeColorTo, _originalColor, elapsedTime / ChangeColorDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _doorRenderer.material.color = _originalColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Si ha completado todos los juegos
            if (_playerAppearGameScreen.AreAllGamesCompleted())
            {
                if (!_completedAllGames)
                {
                    playerData.PlaySoundEffect(doorCorrectSFX);
                    StartCoroutine(AnimationDoorCorrect());
                }
            }

            // Si NO ha completado todos los juegos
            else
            {
                playerData.PlaySoundEffect(doorIncorrectSFX);

                _doorRenderer.material.color = ChangeColorTo;
                StartCoroutine(AnimationDoorIncorrect());
            }
        }
    }
}
