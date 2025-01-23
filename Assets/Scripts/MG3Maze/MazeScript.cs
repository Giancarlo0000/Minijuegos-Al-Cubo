using UnityEngine;

public class MazeScript : MonoBehaviour
{
    [SerializeField] private GameObject MazeGame;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !FindObjectOfType<AppearGameScreen>().IsGameWon(3))
        {
            MazeGame.SetActive(true); // Activa el juego
            FindObjectOfType<PlayerMaze>().SetEnableDisableGame(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !FindObjectOfType<AppearGameScreen>().IsGameWon(3))
        {
            // Resetea el juego
            FindObjectOfType<PlayerMaze>().ResetGame();

            // Desactiva el juego
            FindObjectOfType<PlayerMaze>().SetEnableDisableGame(false);
        }
    }
}
