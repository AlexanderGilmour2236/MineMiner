using UnityEngine;

namespace MineMiner
{
    public class RigidBodyBlockRotationStrategy : BlockRotationStrategy
    {
        [SerializeField] private Rigidbody rigidbody;

        public override void AddPermanentRotation(Vector3 rotation)
        {
            rigidbody.AddTorque(rotation);
        }
    }
}