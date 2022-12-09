using System;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public Action<float> DestroyEvent;
    public Action ResetEvent;
    public AudioSource rumbleAudio;
    public InteriorController interior;

    public void RestartScene()
    {
        ResetEvent?.Invoke();
        interior.Restore();
    }
    public void FallFloor()
    {
        rumbleAudio.Play();
        DestroyEvent?.Invoke(Time.time);
    }
}