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

        [Header("Input")]
        [SerializeField] private float mouseSensitivity;

        private bool _pointerDown;
        private Vector3 _mouseDownPosition = Vector3.zero;
        private Vector2 _mouseDragPosition = Vector2.zero;

        // Starting point of the game
        private void Start()
        {
            InitControllers();
        }

        private void InitControllers()
        {
            blocksController.Init();
            blocksController.BlockDestroy += OnBlockDestroy;
            blocksController.SetAllBlocks(levelParent);
            cameraController.SetTargetPoint(blocksController.LevelCenterTransform);
        }

        private void OnBlockDestroy(BlockView block)
        {
            Destroy(block.gameObject);
        }

        private void Update()
        {
            InputControls();
        }
        
        private void InputControls()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _pointerDown = true;
                _mouseDownPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                _pointerDown = false;
            }

            if (_pointerDown)
            {
                Ray ray = cameraController.Camera.ScreenPointToRay(Input.mousePosition);
                if (!blocksController.TryHitBlock(ray))
                {
                    _mouseDragPosition = Input.mousePosition;
                }
                else
                {
                    _mouseDownPosition = Input.mousePosition;
                    _mouseDragPosition = _mouseDownPosition;
                }
            }

            float xRotation = (_mouseDragPosition.y - _mouseDownPosition.y) * mouseSensitivity;
            float yRotation = (_mouseDragPosition.x - _mouseDownPosition.x) * mouseSensitivity;

            cameraController.Rotate(new Vector3(-xRotation, yRotation));

            _mouseDownPosition = Vector3.Lerp(_mouseDownPosition, _mouseDragPosition, 0.05f);
        }
    }
}