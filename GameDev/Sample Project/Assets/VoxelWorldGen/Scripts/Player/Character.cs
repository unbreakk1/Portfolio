using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Character : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform playerEye;
    [SerializeField] private float interactionRayLength = 5f;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool fly = false;

    [SerializeField] private Animator playerAnim;

    [SerializeField] private bool isWaiting = false;

    [SerializeField] private World world;
    
    [SerializeField] private Volume localVolume;
    [SerializeField] private Volume globalVolume;
    private int speed = Animator.StringToHash("speed");
    private int isGrounded = Animator.StringToHash("isGrounded");
    private int jump = Animator.StringToHash("jump");


    private void Awake()
    {
        if(mainCamera == null)
            mainCamera = Camera.main;
        world = FindObjectOfType<World>();
        localVolume = GetComponentInChildren<Volume>();
       globalVolume = GameObject.FindWithTag("Global Volume").GetComponent<Volume>();
       playerEye = GetComponentInChildren<Transform>();
    }

    private void Start()
    {
        playerInput.OnMouseClick += HandleMouseClick;
        playerInput.OnFly += HandleFlyClick;
    }

    private void HandleFlyClick()
    {
        fly = !fly;
    }

    private void Update()
    {
        if (fly)
        {
            playerAnim.SetFloat(speed, 0);
            playerAnim.SetBool(isGrounded,false);
            playerAnim.ResetTrigger(jump);

            playerMovement.Fly(playerInput.MovementInput, playerInput.IsJumping, playerInput.RunningPressed);
        }
        else
        {
            playerAnim.SetBool(isGrounded, playerMovement.IsGrounded);
            if (playerMovement.IsGrounded && playerInput.IsJumping && isWaiting == false)
            {
                playerAnim.SetTrigger(jump);
                isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(ResetWaiting());
            }
            playerAnim.SetFloat(speed,playerInput.MovementInput.magnitude);
            playerMovement.HandleGravity(playerInput.IsJumping);
            playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);
        }
    }

    private IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        playerAnim.ResetTrigger("jump");
        isWaiting = false;
    }

    private void HandleMouseClick()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
            ModifyTerrain(hit);
    }

    private void ModifyTerrain(RaycastHit hit)
    {
        world.SetBlock(hit, BlockType.Air);
    }
}
