using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float sensitivity = 300f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private PlayerInput playerInput;

    private float verticalRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseX = playerInput.MousePosition.x * sensitivity * Time.deltaTime;
        float mouseY = playerInput.MousePosition.y * sensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -80, 75);
        
        transform.localRotation = Quaternion.Euler(verticalRotation,0,0);
        playerBody.Rotate(Vector3.up*mouseX);
        
    }
}
