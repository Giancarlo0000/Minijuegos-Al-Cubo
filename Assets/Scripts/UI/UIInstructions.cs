using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInstructions : MonoBehaviour
{
    public enum MiniGameType
    {
        Generic,
        Numbers,
        FallingObjects,
        Ring,
        BookSeason
    }

    [SerializeField] private MiniGameType MinigameType = MiniGameType.Generic;
    [SerializeField] private Text TouchToContinue = null;
    [SerializeField] private Text ControlsTextUI = null;

    [SerializeField] private string ControlsPC = string.Empty;
    [SerializeField] private string ControlsMobile = string.Empty;
    [SerializeField] private RectTransform Panel = null;

    private Vector2 _initialPanelPosition = Vector2.zero;
    private float _moveDuration = 0.75f;
    private float _moveAmount = 2000f;
    private bool _isOnScreen = true;

    private void Awake()
    {
#if UNITY_STANDALONE
        ControlsTextUI.text = $"Controles:\n {ControlsPC}";
#else
        ControlsTextUI.text = $"Controles:\n {ControlsMobile}";
#endif
        StartCoroutine(BlinkText());
    }

    private void Update()
    {
        DetectTouch();
    }


    private void DetectTouch()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0) && _isOnScreen)
        {
            Debug.Log("Detectado PC");
            StartCoroutine(MovePanel(_moveAmount));
        }
#else
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && _isOnScreen)
        {
            Debug.Log("Detectado Celular");
            StartCoroutine(MovePanel(_moveAmount));
        }
#endif
    }

    private void StartGame()
    {
        switch (MinigameType)
        {
            case MiniGameType.Generic:
                break;
            case MiniGameType.Numbers:
                MicrojuegoPresionaEnOrden microjuegoPresionaEnOrden = FindObjectOfType<MicrojuegoPresionaEnOrden>();
                microjuegoPresionaEnOrden.SetInGame(true);
                break;
            case MiniGameType.FallingObjects:
                DataGameMG1 dataGameMG1 = FindObjectOfType<DataGameMG1>();
                dataGameMG1.SetIsInGame(true);
                break;
            case MiniGameType.Ring:
                RingGameManager ringGameManager = FindObjectOfType<RingGameManager>();
                ringGameManager.SetIsInGame(true);
                break;
            case MiniGameType.BookSeason:
                SensorBookSeasonScript sensorBookSeasonScript = FindObjectOfType<SensorBookSeasonScript>();
                sensorBookSeasonScript.LoadSceneStartGame();
                break;
        }
    }

    private IEnumerator MovePanel(float direction)
    {
        _isOnScreen = false;
        Vector2 targetPanelPosition = _initialPanelPosition + new Vector2(0, direction);

        float elapsedTime = 0f;
        Vector2 startPanelPosition = Panel.anchoredPosition;

        while (elapsedTime < _moveDuration)
        {
            float t = elapsedTime / _moveDuration;
            Panel.anchoredPosition = Vector2.Lerp(startPanelPosition, targetPanelPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Panel.anchoredPosition = targetPanelPosition;
        StartGame();
    }

    private IEnumerator BlinkText()
    {
        while (true)
        {
            for (float alpha = 1; alpha >= 0; alpha -= Time.deltaTime)
            {
                SetTextAlpha(alpha);
                yield return null;
            }

            for (float alpha = 0; alpha <= 1; alpha += Time.deltaTime)
            {
                SetTextAlpha(alpha);
                yield return null;
            }
        }
    }

    private void SetTextAlpha(float alpha)
    {
        if (TouchToContinue != null)
        {
            Color color = TouchToContinue.color;
            color.a = alpha;
            TouchToContinue.color = color;
        }
    }
}