using UnityEngine;

namespace MineMiner
{
    public interface ICameraMovementStrategy
    {
        void SetTargetPoint(Transform point);
        void Rotate(Vector3 rotation);
        void RotateQuaternion(Quaternion rotation);
        void Tick();
        Camera Camera { get; }
    }
}