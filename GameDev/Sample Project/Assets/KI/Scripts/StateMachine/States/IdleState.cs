using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    private float sqrChaseRadius;
    private int idleRadius;
    private Transform zombieTransform;
    private Animator zAnim;
    private NavMeshAgent agent;
    private AttackingZombie attackingZombie;
    private Vector3 target;
    private int speed = Animator.StringToHash("Speed");

    public IdleState(StateMachine machine) : base(machine)
    {
        attackingZombie = machine.Owner as AttackingZombie;
    }
    public override void Start()
    {
            sqrChaseRadius = attackingZombie.ChaseRadius * attackingZombie.ChaseRadius;
            zombieTransform = attackingZombie.transform;
            agent = attackingZombie.Agent;
            idleRadius = attackingZombie.IdleRadius;
            zAnim = attackingZombie.ZAnim;

            SetRandomDestination();
    }

    public override void Update()
    {
        if(agent.velocity == Vector3.zero)
        {
            SetRandomDestination();
        }
        
        if (Vector3.SqrMagnitude(zombieTransform.position - attackingZombie.PlayerTransform.position) < sqrChaseRadius)
            machine.SwitchState(attackingZombie.ChaseState);
        zAnim.SetFloat(speed, agent.velocity.magnitude);
    }

    private void SetRandomDestination()
    {
        
        Vector3 playerPos = Random.insideUnitCircle * idleRadius;
        target = new Vector3(playerPos.x + zombieTransform.position.x, zombieTransform.position.y, playerPos.y + zombieTransform.position.z);
        if (NavMesh.SamplePosition(target, out NavMeshHit hit, 5, 1))
        {
            agent.SetDestination(hit.position);
        }

        if (Vector3.SqrMagnitude(zombieTransform.position - attackingZombie.PlayerTransform.position) < sqrChaseRadius)
        {
            // switch to ChaseState
        }
    }
}
