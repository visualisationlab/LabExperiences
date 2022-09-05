using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StartAnimation()
    {
        anim.SetTrigger("Start");
    }
    
    public void HideAnimation()
    {
        anim.SetTrigger("Hide");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
