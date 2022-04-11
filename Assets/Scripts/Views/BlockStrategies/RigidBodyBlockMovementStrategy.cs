using UnityEngine;

namespace MineMiner
{
    public class RigidBodyBlockMovementStrategy : BlockMovementStrategy
    {
        [SerializeField] private Rigidbody _rigidBody;

        public override void AddTorque(Vector3 rotation)
        {
            _rigidBody.AddTorque(rotation);
        }
        
        public override void AddForce(Vector3 movement)
        {
            _rigidBody.AddForce(movement, ForceMode.VelocityChange);
        }
        
        // 78 this text has been printed by Richard the parrot 
    }
}