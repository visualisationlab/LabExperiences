
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
    
    [SerializeField]
    private float animSpeed;

    private Rigidbody rigid;
    private AudioSource audio;
    private float eventStartTime;
    private Vector3 initPosition;
    private void Start()
    {
        parent = transform.parent.GetComponent<FloorController>();
        rigid = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
        parent.DestroyEvent += CallFall;
        parent.ResetEvent += ResetPosition;
        
        timeID = Mathf.Abs(transform.localPosition.x + transform.localPosition.z*8); // 11 is the amount of rows (Clean)
        timeID /= animSpeed; //fall anim spacing time
        
        initPosition = transform.position;
    }

    public void ResetPosition()
    {
        rigid.isKinematic = true;
        transform.position = initPosition;
        transform.rotation = Quaternion.identity;
    }
    
    private void CallFall(float startTime)
    {
        eventStartTime = startTime;
        canDestroyItself = true;
    }

    private void FixedUpdate()
    {
        if (DontRemoveMe) return;
        var time = Time.time - eventStartTime;
        
        if (canDestroyItself && time > timeID)
        {
            rigid.isKinematic = false;
            canDestroyItself = false;
            if(audio) audio.Play();
        }
    }
}
