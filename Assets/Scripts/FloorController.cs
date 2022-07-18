using System;
using System.Collections;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public float animationSpeed;
    public Action<float, float> DestroyEvent;
    public AudioSource rumbleAudio;

    private void Start()
    {
        StartCoroutine(TimeDestroyFloor());
    }

    private IEnumerator TimeDestroyFloor()
    {
        yield return new WaitForSeconds(5f);
        DestroyFloor();
    }

    public void DestroyFloor()
    {
        DestroyEvent?.Invoke(Time.time * animationSpeed, animationSpeed);
        rumbleAudio.Play();
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