using Misc;
using UnityEngine;

namespace MineMiner
{
    public class AroundCameraController : MonoBehaviour, ICameraController
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private Vector3 cameraStartOffset;
        
        private Transform lookTargetPoint;
        
        private void Start()
        {
            camera.transform.SetParent(cameraPivot, true);
        }

        public void SetTargetPoint(Transform point)
        {
            lookTargetPoint = point;
        }

        private void Update()
        {
            if (lookTargetPoint != null)
            {
                camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, Quaternion.LookRotation(lookTargetPoint.position - camera.transform.position), 0.05f );
            }
        }

        public Camera Camera
        {
            get { return camera; }
        }

        public void Rotate(Vector3 rotation)
        {
            cameraPivot.transform.eulerAngles += rotation;
            
            Vector3 angles = cameraPivot.transform.localEulerAngles;
            angles.x = angles.x.ClampAngle(-40, 40);
            cameraPivot.transform.localEulerAngles = angles;
        }
        
        public void RotateQuaternion(Quaternion rotation)
        {
            cameraPivot.transform.rotation = Quaternion.identity * rotation;
            
            Vector3 angles = cameraPivot.transform.localEulerAngles;
            angles.x = angles.x.ClampAngle(-40, 40);
            cameraPivot.transform.localEulerAngles = angles;
        }
    }
}