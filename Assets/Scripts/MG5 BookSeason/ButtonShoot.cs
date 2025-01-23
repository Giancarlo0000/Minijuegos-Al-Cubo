using UnityEngine;

public class ButtonShoot : MonoBehaviour
{
    [SerializeField] private Sniper sniperScript;

    public void SendAndroidMessage()
    {
        if (sniperScript != null)
        {
            sniperScript.SendMessage("Shoot");
        }
        else
        {
            Debug.LogError("SniperScript no está asignado en ButtonShoot.");
        }
    }
}
