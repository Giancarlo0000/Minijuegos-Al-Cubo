using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEndGameplay : MonoBehaviour
{
    // La puerta
    [Header("Door")] // Borrrar?
    [SerializeField] private Transform DoorTrfm = null;
    private Vector3 _doorInitialPosition = Vector3.zero;
    [SerializeField] private Vector3 ModificationPositionValue = Vector3.zero;
    [SerializeField] private float DistanceUpMove;
    [SerializeField] private float DurationMove;

    [SerializeField] private GameObject PlayerGameobject;

    void Start()
    {
        _doorInitialPosition = DoorTrfm.position;
    }

    void Update()
    {

    }

    private IEnumerator OpenDoorAnimation()
    {
        float elapsedTime = 0;
        Vector3 origin = _doorInitialPosition;
        Vector3 target = _doorInitialPosition + ModificationPositionValue;

        while (elapsedTime < DurationMove)
        {
            // Movimiento a través de un Lerp
            elapsedTime += Time.deltaTime;
            DoorTrfm.position = Vector3.Lerp(origin, target, elapsedTime / DurationMove);

            yield return null;
        }
    }
}
