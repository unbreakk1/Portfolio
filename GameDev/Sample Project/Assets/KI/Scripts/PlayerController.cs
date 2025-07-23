using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Animator pAnim;

    private bool isActive;
    private Rigidbody rigid;
    private Vector2 moveDirection;
    private Vector2 moveInput;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;

    }
    private void Update()
    {
        pAnim.SetFloat("Speed", rigid.velocity.magnitude);
    }
    private void FixedUpdate()
    {
        Movement();
    }
    
    public void MoveInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            moveInput = ctx.ReadValue<Vector2>();
        }
    }

    private void Movement()
    {
        if (moveInput == Vector2.zero || !isActive)
        {
            return;
        }

        moveDirection = moveInput * moveSpeed * Time.fixedDeltaTime;
        
        rigid.velocity = new Vector3(moveDirection.x, rigid.velocity.y, moveDirection.y);
        Vector3 targetForward = new Vector3(moveDirection.x, 0, moveDirection.y);
        transform.forward = targetForward;
    }
}
