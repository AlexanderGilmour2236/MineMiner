using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MineMiner
{
    public class CameraRaycastController
    {
        private Camera _camera;
        private LayerMask _layerMask;
        private bool _enabled;
        
        public event Action<RaycastHit> onHit;

        public CameraRaycastController(Camera camera, LayerMask layerMask)
        {
            _camera = camera;
            _layerMask = layerMask;
        }

        public void SetIsEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        
        public void Tick()
        {
            if (!_enabled || !Input.GetMouseButton(0)){
                return;
            }
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, _layerMask))
            {
                onHit?.Invoke(hit);
            }
        }
    }
}