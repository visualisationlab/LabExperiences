using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    [SerializeField] private FloorController floorController;
    [SerializeField] private GameObject colliderTarget;
    private bool initiated;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!initiated && other.gameObject == colliderTarget)
        {
            initiated = true;
            floorController.FallFloor();
        }
    }

    public void ResetMe()
    {
        initiated = false;
    }
    
    
}
