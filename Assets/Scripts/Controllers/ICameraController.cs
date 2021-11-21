using UnityEngine;

namespace MineMiner
{
    public interface ICameraController
    {
        void SetTargetPoint(Transform point);
        void Rotate(Vector3 rotation);
        void RotateQuaternion(Quaternion rotation);
        Camera Camera { get; }
    }
}