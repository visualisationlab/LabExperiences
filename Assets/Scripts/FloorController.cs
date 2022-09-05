using System;
using System.Collections;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public float animationSpeed;
    public Action<float> DestroyEvent;
    public Action ResetEvent;
    public AudioSource rumbleAudio;
    private void Start()
    {
        StartCoroutine(TimeDestroyFloor());
        rumbleAudio.Play();
    }

    private IEnumerator TimeDestroyFloor()
    {
        yield return new WaitForSeconds(2f);
        FallFloor();
        yield return new WaitForSeconds(20f);
        ResetEvent?.Invoke();
        RestartTimer();
    }

    private void RestartTimer()
    {
        StartCoroutine(TimeDestroyFloor());
    }

    public void FallFloor()
    {
        DestroyEvent?.Invoke(Time.time);
    }
}

/*
[CustomEditor(typeof(FloorController))]
[CanEditMultipleObjects]
public class FloorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var t = (target as FloorController);

        if (GUILayout.Button("Init Destroy"))
        {
            t.DestroyFloor();
        }
    }
}*/