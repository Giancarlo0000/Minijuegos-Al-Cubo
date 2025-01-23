using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string SceneChangeName = string.Empty;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) SceneManager.LoadScene(SceneChangeName);
    }

    public void ChangeSceneButton(string SceneName) => SceneManager.LoadScene(SceneName);
    public void ExitGame() => Application.Quit();
}
