using UnityEngine;
using UnityEngine.AI;

public class FleeState : State
{
    private float sqrEvadeRadius;
    private Transform playerTransform;
    private Transform zombieTransform;
    private FleeingZombie fleeingZombie;
    private Animator zAnim;
    private NavMeshAgent agent;

    public FleeState(StateMachine machine) : base(machine) 
    {
        fleeingZombie = machine.Owner as FleeingZombie;
    }
    public override void Start()
    {
        sqrEvadeRadius = fleeingZombie.EvadeRadius * fleeingZombie.EvadeRadius;
        zombieTransform = fleeingZombie.transform;
        playerTransform = fleeingZombie.PlayerTransform;
        zAnim = fleeingZombie.ZAnim;
        agent = fleeingZombie.Agent;
    }

    public override void Update()
    {
        Vector3 direction = fleeingZombie.transform.position - fleeingZombie.PlayerTransform.position;
       
        agent.SetDestination(fleeingZombie.transform.position + direction);
        zAnim.SetFloat("Speed", agent.velocity.magnitude);

        if(Vector3.SqrMagnitude(zombieTransform.position - playerTransform.position) > sqrEvadeRadius)
        {
            machine.SwitchState(fleeingZombie.IdleState);
        }
    }
}
