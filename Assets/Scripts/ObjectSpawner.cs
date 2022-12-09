using System.Collections;
using System.Collections.Generic;
using Ubiq.Spawning;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject visBall;
    void Start()
    {
        StartCoroutine(Waiter());
    }

    IEnumerator Waiter()
    {
        yield return new WaitForSeconds(1f);
        var ball = NetworkSpawnManager.Find(this).SpawnWithPeerScope(visBall);
        ball.transform.position = transform.position;
    }

}
