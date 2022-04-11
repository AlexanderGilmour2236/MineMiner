using UnityEngine;

namespace MineMiner
{
    public class BlockMovementStrategy : MonoBehaviour
    {
        public virtual void AddForce(Vector3 movement)
        {
        }
        
        public virtual void AddTorque(Vector3 rotation)
        {
        }
    }
}