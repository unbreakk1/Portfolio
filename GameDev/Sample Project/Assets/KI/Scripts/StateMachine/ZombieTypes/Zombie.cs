using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class Zombie : MonoBehaviour
{
    
    [SerializeField] private int idleRadius;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Animator zAnim;
    protected StateMachine brain;

    protected void Start()
    {
        brain = new StateMachine(this);
        InitializeState();
    }

    protected abstract void InitializeState();
    protected void Update()
    {
        brain.Update();
    }

    public LayerMask PlayerMask => playerMask;
    public NavMeshAgent Agent => agent;
    public int IdleRadius => idleRadius;
    public Animator ZAnim => zAnim;
    public Transform PlayerTransform => playerTransform;
}
