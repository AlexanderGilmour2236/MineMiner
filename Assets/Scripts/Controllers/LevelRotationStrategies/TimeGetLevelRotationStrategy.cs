using UnityEngine;

namespace MineMiner
{
    public class TimeGetLevelRotationStrategy : IGetLevelRotationStrategy
    {
        private readonly float _rotationSpeed;

        public TimeGetLevelRotationStrategy(float rotationSpeed)
        {
            _rotationSpeed = rotationSpeed;
        }

        public Vector3 GetRotation(LevelCameraController levelCameraController)
        {
            return new Vector3(0, Time.deltaTime * _rotationSpeed);;
        }
    }
}