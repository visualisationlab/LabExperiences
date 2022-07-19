
using System;
using System.Collections;
using UnityEngine;

public class FloorItem:MonoBehaviour
{
    private FloorController parent;
    
    private float timeID;
    
    private bool canDestroyItself;

    [SerializeField]
    private bool DontRemoveMe;

    private Rigidbody rigid;
    
    private AudioSource audio;
    
    private float eventStartTime;
    private float animSpeed;
    private void Start()
    {
        parent = transform.parent.GetComponent<FloorController>();
        timeID = Mathf.Abs(transform.localPosition.x + transform.localPosition.z*8); // 11 is the amount of rows (Clean)
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        parent.DestroyEvent += CallDestroy;
    }

    private void CallDestroy(float startTime, float animationSpeed)
    {
        animSpeed = animationSpeed;
        eventStartTime = startTime;
        canDestroyItself = true;
    }

    private void FixedUpdate()
    {
        if (DontRemoveMe) return;
        var time = Time.time * animSpeed;
        if (canDestroyItself && (time-eventStartTime) > timeID)
        {
            rigid.isKinematic = false;
            canDestroyItself = false;
            if(audio) audio.Play();
            // transform.position += Vector3.down * 0.1f;
            // Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        parent.DestroyEvent -= CallDestroy;
    }
}
