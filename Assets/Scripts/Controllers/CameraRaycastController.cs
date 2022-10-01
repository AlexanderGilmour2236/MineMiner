using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace MineMiner
{
    public class CameraRaycastController
    {
        private Camera _camera;
        private LayerMask _layerMask;
        
        public event Action<RaycastHit> onHit;

        public CameraRaycastController(Camera camera, LayerMask layerMask)
        {
            _camera = camera;
            _layerMask = layerMask;
        }
        
        public void Tick()
        {
            if (!Input.GetMouseButton(0)){
                return;
            }
            
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 500, _layerMask))
            {
                Debug.Log("hit");
                onHit?.Invoke(hit);
            }
        }
    }
}