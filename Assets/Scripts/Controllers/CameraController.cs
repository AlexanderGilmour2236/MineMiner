using UnityEngine;

namespace Game.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        public Camera Camera
        {
            get { return camera; }
        }
    }
}