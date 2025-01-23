using UnityEngine;

public class WinConditionBall : MonoBehaviour
{
    [SerializeField] private int ballsToWin = 3;
    private int _points = 0;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private AudioClip AddPointSFX = null;
    [SerializeField] private AudioClip ResetSFX = null;
    private void OnTriggerEnter(Collider other)
    {
        //Con meter 3 bolas verdes se gana
        if (other.CompareTag("GreenBall")) //Deben estar los tags para GreenBall y RedBall
        {
            _points++;
            playerData.PlaySoundEffect(AddPointSFX);

            if (_points >= ballsToWin)
            {
                // Marca el microjuego como ganado.
                FindObjectOfType<AppearGameScreen>().MarkGameAsWon(4);

                // Cierra el panel del microjuego
                FindObjectOfType<AppearGameScreen>().CloseMicrogameWindow();

                // Desactiva el juego
                FindObjectOfType<InsertBallEnableDisable>().EnableGameobject(false);
            }
        }
        //Se resetea si una bola roja entra en el agujero
        if (other.CompareTag("RedBall"))
        {
            playerData.PlaySoundEffect(ResetSFX);

            FindObjectOfType<InstantiateBalls>().EliminateBalls();
            FindObjectOfType<InstantiateBalls>().CreateBalls();
            _points = 0;
        }
    }
}
