using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Platform.Models;
using Ubiq.Messaging;
using Ubiq.Rooms;
using Ubiq.Spawning;
using Ubiq.XR;
using UnityEngine;
using UnityEngine.XR;
using Hand = Ubiq.XR.Hand;

public class VisBall : MonoBehaviour, INetworkSpawnable, IGraspable
{
    public NetworkId NetworkId { get; set; }
    public List<Pose> GrabPoints { get; }
    public Transform Transform { get; }

    public Rigidbody rigid;
    
    public RoomClient RoomClient { get; private set; }
    
    NetworkContext context;
    Vector3 lastPosition;
    
    private Vector3 localGrabPoint;
    private Quaternion localGrabRotation;
    private Quaternion grabHandRotation;
    private Vector3 handPos;
    private Vector3 prevPos;
    private Transform follow;

    private int followStep = 0;
    private int followStepThreshold = 100;
    
    ClaimBall ownerClaim = new ClaimBall{ownerUUID = "", protect = false};

    private void Awake()
    {
        RoomClient = GetComponentInParent<RoomClient>();
    }

    // Start is called before the first frame update
    void Start()
    {
        context = NetworkScene.Register(this);
    }
 
    public void Grasp(Hand controller)
    {
        if (ownerClaim.protect) return; //is currently already grasped
        
        MakeClaim(RoomClient.Me.uuid, true);
        followStep = 0;

        InputDevice device = new InputDevice();
        
        
        Physics.IgnoreLayerCollision(3, 6, true);
        var handTransform = controller.transform;
        localGrabPoint = handTransform.InverseTransformPoint(transform.position); //transform.InverseTransformPoint(handTransform.position);
        localGrabRotation = Quaternion.Inverse(handTransform.rotation) * transform.rotation;
        grabHandRotation = handTransform.rotation;

        rigid.isKinematic = true;
        follow = handTransform;
    }
    
    //Could do some interpolation between multiple points to get a more accurate feel for what the ball should do on release.
    //For now it feels okay-ish
    public void Release(Hand controller)
    {
        
        MakeClaim(RoomClient.Me.uuid, false); //release for others to claim
        
        var diff = handPos - prevPos;
        rigid.isKinematic = false;
        rigid.AddForce(diff * 20f, ForceMode.VelocityChange);
        follow = null;
        StartCoroutine(TimedRelease());
    }

    private IEnumerator TimedRelease()
    {
        yield return new WaitForSeconds(0.8f);
        Physics.IgnoreLayerCollision(3, 6, false);
    }

    void MakeClaim(string uuid, bool protect)
    {
        ownerClaim = new ClaimBall()
        {
            ownerUUID = uuid,
            protect = protect
        };
        
        Debug.Log("Sending Owner Claim information");
        context.SendJson(ownerClaim);
    }
    

    // Update is called once per frame
    void Update()
    {
        
        //Grasped
        if (follow)
        {
            transform.rotation = follow.rotation * localGrabRotation;
            transform.position = follow.TransformPoint(localGrabPoint);
            handPos = follow.position;
            if (followStep > followStepThreshold)
            {
                prevPos = handPos;
                followStep = 0;
            }
        }

        if (AmIOwner())
        {
            context.SendJson(new PositionRotation()
            {
                position = transform.localPosition
            });
        }

        followStep++;
    }

    private bool AmIOwner()
    {
        return ownerClaim.ownerUUID == RoomClient.Me.uuid;
    }
    
    
    private struct ClaimBall
    {
        public string ownerUUID;
        public bool protect;
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
        var ownerM = message.FromJson<ClaimBall>();

        if (!String.IsNullOrEmpty(ownerM.ownerUUID))
        {
            ownerClaim = ownerM;
            Debug.Log($"Owner Claimage ID: {ownerClaim.ownerUUID} Protected? {ownerClaim.protect}");
        }
        
        if (!AmIOwner())// I'm not the owner follow from owner
        {
            
            rigid.isKinematic = true;
            var m = message.FromJson<PositionRotation>();
            transform.localPosition = m.position;
            transform.localRotation = m.rotation;
        }
    }

}
