using Game.Block;
using UnityEngine;

namespace Game.Controllers
{
    public class MainController : MonoBehaviour
    {
        
        [Header("Controllers")]
        [SerializeField] private CameraController cameraController;
        [SerializeField] private BlocksController blocksController;

        [Header("Level")] 
        [SerializeField] private Transform levelParent;

        [Header("Tags")] 
        [SerializeField] private string blocksTag = "Block";

        private bool _pointerDown;

        private void Start()
        {
            blocksController.BlockDestroy += OnBlockDestroy;
            blocksController.SetAllBlocks(levelParent);
        }

        private void OnBlockDestroy(BlockView block)
        {
            
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pointerDown = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _pointerDown = false;
            }

            if (_pointerDown)
            {
                Ray ray = cameraController.Camera.ScreenPointToRay(Input.mousePosition);
                blocksController.TryHitBlock(ray);
            }
        }
    }
}