
using UnityEngine;

namespace MineMiner
{
    public interface IGetLevelRotationStrategy
    {
        public  Vector3 GetRotation(LevelCameraController levelCameraController);
    }
}