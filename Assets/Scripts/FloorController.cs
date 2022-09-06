using System;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public Action<float> DestroyEvent;
    public Action ResetEvent;
    public AudioSource rumbleAudio;
    public InteriorController interior;

    private void Start()
    {
        rumbleAudio.Play();
    }

    public void RestartScene()
    {
        ResetEvent?.Invoke();
        interior.Restore();
        rumbleAudio.Play();
    }
    public void FallFloor()
    {
        DestroyEvent?.Invoke(Time.time);
    }
}