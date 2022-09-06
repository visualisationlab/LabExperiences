using System;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private Tuple<Vector3, Quaternion> original;

    [SerializeField] private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        original = new Tuple<Vector3, Quaternion>(transform.position, transform.rotation);
    }

    public void GrabMe()
    {
        
    }

    public void ReleaseMe()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    
    public void ResetMe()
    {
        rigid.isKinematic = true;
        transform.position = original.Item1;
        transform.rotation = original.Item2;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.isKinematic = false;

    }
}
