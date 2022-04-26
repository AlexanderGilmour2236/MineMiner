using UnityEngine;
using UnityEngine.EventSystems;

public class CursorLocker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CursorLockMode _startLockMode = CursorLockMode.None;
    [SerializeField] private BoxCollider _mouseCollider;

    void Start()
    {
        Cursor.lockState = _startLockMode;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            ShowCollider();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HideCollider();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HideCollider()
    {
        Vector3 colliderCenter = _mouseCollider.center;
        colliderCenter.y = -1000;
        _mouseCollider.center = colliderCenter;
    }
    
    private void ShowCollider()
    {
        Vector3 colliderCenter = _mouseCollider.center;
        colliderCenter.y = 0;
        _mouseCollider.center = colliderCenter;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
