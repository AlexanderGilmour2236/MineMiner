using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorLocker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CursorLockMode _startLockMode = CursorLockMode.None;
    
    void Start()
    {
        Cursor.lockState = _startLockMode;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
