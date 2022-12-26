using UnityEngine;

namespace MineMiner
{
    public class DragGetLevelRotationStrategy : IGetLevelRotationStrategy
    {
        public Vector3 GetRotation(LevelCameraController levelCameraController)
        {
            float xRotation = (levelCameraController.MouseDragPosition.y - levelCameraController.MouseDownPosition.y) * levelCameraController.MouseSensitivity;
            float yRotation = (levelCameraController.MouseDragPosition.x - levelCameraController.MouseDownPosition.x) * levelCameraController.MouseSensitivity;

            return new Vector3(-xRotation, yRotation);;
        }
    }
}