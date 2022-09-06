using System;
using System.Collections.Generic;
using UnityEngine;

public class InteriorController : MonoBehaviour
{
    private Dictionary<Transform, Tuple<Vector3, Quaternion>> positions;
    void Start()
    {
        positions = new Dictionary<Transform, Tuple<Vector3, Quaternion>>();
        foreach (var cp in transform.GetComponentsInChildren(typeof(Transform)))
        {
            var tr = cp as Transform;
            positions.Add(tr, new Tuple<Vector3, Quaternion>(tr.position, tr.rotation));
        }
    }

    public void Restore()
    {
        foreach (var p in positions)
        {
            var rigid = p.Key.GetComponent<Rigidbody>();
            if(rigid) rigid.isKinematic = true;
            p.Key.position = p.Value.Item1;
            p.Key.rotation = p.Value.Item2;
            if(rigid) rigid.isKinematic = false;
        }
    }
}
