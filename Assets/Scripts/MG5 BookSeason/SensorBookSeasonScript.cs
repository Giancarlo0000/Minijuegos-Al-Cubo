using UnityEngine;
using UnityEngine.SceneManagement;

public class SensorBookSeasonScript : MonoBehaviour
{
    [SerializeField] private string MicrogameSceneName = string.Empty;
    private AppearGameScreen _appearGameScreen = null;
    private int _count = 1;
    private bool _isGameWin = false;

    private void Start()
    {
        _appearGameScreen = FindObjectOfType<AppearGameScreen>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            _isGameWin = FindObjectOfType<GlobalGameData>().GetIsCompleted(4);

            if (_isGameWin && _count >= 1)
            {
                _appearGameScreen.MarkGameAsWon(5);
                _count--;
            }
        }
    }

    public void LoadSceneStartGame() 
    {
        if (!_isGameWin) SceneManager.LoadScene(MicrogameSceneName);
    }
}
