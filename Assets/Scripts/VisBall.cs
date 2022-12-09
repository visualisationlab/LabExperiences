using System.Collections.Generic;
using Oculus.Interaction;
using Ubiq.Messaging;
using Ubiq.Spawning;
using Ubiq.XR;
using UnityEngine;

public class VisBall : MonoBehaviour, INetworkSpawnable, IGraspable
{
    public NetworkId NetworkId { get; set; }
    public List<Pose> GrabPoints { get; }
    public Transform Transform { get; }

    public Rigidbody rigid;
    
    
    NetworkContext context;
    Vector3 lastPosition;
    
    private Vector3 localGrabPoint;
    private Quaternion localGrabRotation;
    private Quaternion grabHandRotation;
    private Transform prevFollow;
    private Transform follow;

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);   
    }
 
    public void Grasp(Hand controller)
    {
        rigid.isKinematic = true;
        var handTransform = controller.transform;
        localGrabPoint = handTransform.InverseTransformPoint(transform.position); //transform.InverseTransformPoint(handTransform.position);
        localGrabRotation = Quaternion.Inverse(handTransform.rotation) * transform.rotation;
        grabHandRotation = handTransform.rotation;
        follow = handTransform;
    }
    
    
    public void Release(Hand controller)
    {
        var difference = follow.position - prevFollow.position;

        rigid.isKinematic = false;
        rigid.AddForce(difference * 20f, ForceMode.VelocityChange);

        follow = null;
        prevFollow = null;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        //Grasped
        if (follow)
        {
            transform.rotation = follow.rotation * localGrabRotation;
            transform.position = follow.TransformPoint(localGrabPoint);
            prevFollow = follow;
        }
        
        //Runs when physics is active
        if(!rigid.isKinematic){
            context.SendJson(new PositionRotation()
            {
                position = transform.localPosition
            });
        }
    }
    
    private struct PositionRotation
    {
        public Vector3 position;
        public Quaternion rotation;

        public static PositionRotation identity
        {
            get
            {
                return new PositionRotation
                {
                    position = Vector3.zero,
                    rotation = Quaternion.identity
                };
            }
        }
    }
    
    public void ProcessMessage(ReferenceCountedSceneGraphMessage message)
    {
        //update when physics comes from other peer
        if (rigid.isKinematic)
        {
            var m = message.FromJson<PositionRotation>();
            transform.localPosition = m.position;
            transform.localRotation = m.rotation;
        }
    }

}
