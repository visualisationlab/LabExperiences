using System;
using System.Linq;
using UnityEngine;

namespace Ubiq.XR
{
    /// <summary>
    /// This VR Player Controller supports a typical Head and Two Hand tracked rig.
    /// </summary>
    public class XRCustomPlayerController : MonoBehaviour
    {
        public bool dontDestroyOnLoad = true;

        public Rigidbody rigid;

        public float rotateSpeed = 1f;
        public float moveSpeed = 1f;

        private static XRCustomPlayerController singleton;

        public static XRCustomPlayerController Singleton
        {
           get { return singleton; }
        }

        [NonSerialized]
        public HandController[] handControllers;

        private Vector3 velocity;
        private Vector3 userLocalPosition;

        public float joystickDeadzone = 0.1f;
        public float joystickFlySpeed = 1.2f;

        public Camera headCamera;
        public Transform cameraContainer;
        public AnimationCurve cameraRubberBand;

        private void Awake()
        {
            if(dontDestroyOnLoad)
            {
                if(singleton != null)
                {
                    gameObject.SetActive(false);
                    DestroyImmediate(gameObject);
                    return;
                }

                singleton = this;
                DontDestroyOnLoad(gameObject);
                Extensions.MonoBehaviourExtensions.DontDestroyOnLoadGameObjects.Add(gameObject);
            }

            handControllers = GetComponentsInChildren<HandController>();
        }


        private void Update()
        {
            // Update the foot position. This is done by pulling the feet using a rubber band.
            // Decoupling the feet in this way allows the user to do things like lean over edges, when the ground check is enabled.
            // This can be effectively disabled by setting the animation curve to a constant high value.

            foreach (var item in handControllers)
            {
                if (item.Right)
                {
                    if (Mathf.Abs(item.Joystick.x) > 0.1f)
                    {
                        transform.RotateAround(headCamera.transform.position, Vector3.up, item.Joystick.x * rotateSpeed);
                    }
                }
                if (item.Left)
                {
                    var dir = item.Joystick.normalized;
                    var mag = item.Joystick.magnitude;
                    if (mag > joystickDeadzone)
                    {
                        var joyStick = new Vector3(dir.x, 0, dir.y) * mag;
                        var headDir = Quaternion.Euler(new Vector3(0, headCamera.transform.rotation.eulerAngles.y,0 )) * joyStick;
                        rigid.AddForce(headDir * moveSpeed, ForceMode.VelocityChange);
                    }
                }
            }

            var headProjectionXZ = transform.InverseTransformPoint(headCamera.transform.position);
            headProjectionXZ.y = 0;
            userLocalPosition.x += (headProjectionXZ.x - userLocalPosition.x) * Time.deltaTime * cameraRubberBand.Evaluate(Mathf.Abs(headProjectionXZ.x - userLocalPosition.x));
            userLocalPosition.z += (headProjectionXZ.z - userLocalPosition.z) * Time.deltaTime * cameraRubberBand.Evaluate(Mathf.Abs(headProjectionXZ.z - userLocalPosition.z));
            userLocalPosition.y = 0;
        }

        private void OnDrawGizmos()
        {
            if (!headCamera) {
                return;
            }

            Gizmos.color = Color.blue;
            Gizmos.matrix = transform.localToWorldMatrix;
           //Gizmos.DrawWireCube(Vector3.zero, new Vector3(Radius, 0.1f, Radius));
            Gizmos.DrawLine(userLocalPosition, transform.InverseTransformPoint(headCamera.transform.position));
        }
    }
}
