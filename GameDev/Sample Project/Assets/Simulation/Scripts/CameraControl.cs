using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private CameraCointrolActions cameraActions;
    private InputAction movement;
    
    
    [Header("Horizontal Movement")]
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float speed;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15f;

    [Header("Zoom")] 
    [SerializeField] private float stepSize = 2f;
    //[SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    //[SerializeField] private float zoomSpeed = 2f;

    [Header("Rotation")] 
    [SerializeField] private float maxRotationSpeed = 1f;

    [Header("Screen Edge")] 
    [SerializeField, Range(0f, 0.1f)] private float edgeTolerance = 0.05f;
    [SerializeField] private bool useScreenEdge = true; // only for editing while in playmode
    
    // Camera stuff used in functions/methods
    private Transform cameraTransform;
    private Vector3 targetPosition;
    private float zoomHeight;
    private Vector3 cameraDirection;
    
    //tracking and maintaining velocity w/o rigidbody
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;
    
    // tracking for dragging action
    private Vector3 startDrag;

    private void Awake()
    {
        cameraActions = new CameraCointrolActions();
        cameraTransform = GetComponentInChildren<Camera>().transform;
        cameraDirection = (cameraTransform.position - transform.position).normalized;
        zoomHeight = maxHeight;
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);
        
        lastPosition = transform.position;
        movement = cameraActions.Camera.Movement;
        cameraActions.Camera.Enable();
        cameraActions.Camera.RotateCamera.performed += RotateCamera;
        cameraActions.Camera.ZoomCamera.performed += ZoomCamera;
    }

    private void OnDisable()
    {
        cameraActions.Camera.RotateCamera.performed -= RotateCamera;
        cameraActions.Camera.ZoomCamera.performed -= ZoomCamera;
        cameraActions.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();
        UpdateVelocity();
        UpdateCameraPosition();
        
        if(useScreenEdge)
            CheckMouseAtScreenEdge();
        
        DragCamera();
        
        UpdateBasePosition();
        
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>().x * GetCameraRight()
                             + movement.ReadValue<Vector2>().y * GetCameraForward();

        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f) targetPosition += inputValue;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime * acceleration);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }

        targetPosition = Vector3.zero;
    }
    private void RotateCamera(InputAction.CallbackContext ctx)
    {
        if (!Mouse.current.middleButton.isPressed) return;

        float value = ctx.ReadValue<Vector2>().x;
        cameraDirection = Quaternion.AngleAxis(value * maxRotationSpeed, Vector3.up) * cameraDirection;
    }

    private void ZoomCamera(InputAction.CallbackContext ctx)
    {
        float value = -ctx.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.0001f)
        {
            zoomHeight = zoomHeight + value * stepSize;
            if (zoomHeight < minHeight)
                zoomHeight = minHeight;
            else if (zoomHeight > maxHeight)
                zoomHeight = maxHeight;
        }
    }

    private void UpdateCameraPosition()
    {
      cameraTransform.position = transform.position + cameraDirection * zoomHeight;
        cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();

        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }

    private void DragCamera()
    {
        if (!Mouse.current.rightButton.isPressed) 
            return;

        Plane plane = new Plane(Vector3.up, Vector3.zero);

        
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (plane.Raycast(ray, out float distance))
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
                startDrag = ray.GetPoint(distance);
            else
                targetPosition += startDrag - ray.GetPoint(distance);
        }
    }
}
