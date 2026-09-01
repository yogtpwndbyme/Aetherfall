using UnityEngine;
using UnityEngine.InputSystem;

public class RTSCameraController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;

    [Header("Zoom")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minHeight = 6f;
    [SerializeField] private float maxHeight = 20f;

    [Header("Camera Bounds")]
    [SerializeField] private float minX = -40f;
    [SerializeField] private float maxX = 40f;
    [SerializeField] private float minZ = -40f;
    [SerializeField] private float maxZ = 40f;

    void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampPosition();
    }

    private void HandleMovement()
    {
        Vector3 movement = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            movement.z += 1f;

        if (Keyboard.current.sKey.isPressed)
            movement.z -= 1f;

        if (Keyboard.current.aKey.isPressed)
            movement.x -= 1f;

        if (Keyboard.current.dKey.isPressed)
            movement.x += 1f;

        movement = movement.normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float scroll = Mouse.current.scroll.ReadValue().y;

        if (scroll == 0f)
            return;

        Vector3 zoomDirection = transform.forward;

        Vector3 newPosition =
            transform.position + zoomDirection * scroll * zoomSpeed * 0.01f;

        if (newPosition.y >= minHeight && newPosition.y <= maxHeight)
        {
            transform.position = newPosition;
        }
    }

    private void ClampPosition()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        transform.position = position;
    }
}