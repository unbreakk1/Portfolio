using UnityEngine;
using UnityEngine.AI;

public class FleeingIdle : State
{
    private float sqrEvadeRadius;
    private int idleRadius;
    private Transform zombieTransform;
    private Animator zAnim;
    private NavMeshAgent agent;
    private FleeingZombie fleeingZombie;
    private Vector3 target;


    public FleeingIdle(StateMachine machine) : base(machine)
    {
        fleeingZombie = machine.Owner as FleeingZombie;
    }
    public override void Start()
    {
        sqrEvadeRadius = fleeingZombie.EvadeRadius * fleeingZombie.EvadeRadius;
        zombieTransform = fleeingZombie.transform;
        agent = fleeingZombie.Agent;
        idleRadius = fleeingZombie.IdleRadius;
        zAnim = fleeingZombie.ZAnim;

        SetRandomDestination();
    }

    public override void Update()
    {
        if (Vector3.SqrMagnitude(zombieTransform.position - fleeingZombie.PlayerTransform.position) < sqrEvadeRadius)
        {
            machine.SwitchState(fleeingZombie.FleeState);
        }
        zAnim.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void SetRandomDestination()
    {
        Vector3 playerPos = Random.insideUnitCircle * idleRadius;
        target = new Vector3(playerPos.x + zombieTransform.position.x, zombieTransform.position.y, playerPos.y + zombieTransform.position.z);
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 5, 1))
        {
            agent.SetDestination(hit.position);
            
        }
    }
}
