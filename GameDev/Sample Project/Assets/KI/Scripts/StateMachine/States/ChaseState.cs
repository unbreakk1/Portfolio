using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private float sqrChaseRadius;
    private float sqrAttackRadius;
    private Transform zombieTransform;
    private NavMeshAgent agent;
    private Animator zAnim;
    private Transform playerTransform;
    private AttackingZombie attackingZombie;
    private static readonly int Speed = Animator.StringToHash("Speed");

    public ChaseState(StateMachine machine) : base(machine)
    {
        attackingZombie = machine.Owner as AttackingZombie;
    }
    
    public override void Start()
    {
        sqrChaseRadius = attackingZombie.ChaseRadius * attackingZombie.ChaseRadius;
        sqrAttackRadius = attackingZombie.AttackRadius * attackingZombie.AttackRadius;
        zombieTransform = attackingZombie.transform;
        agent = attackingZombie.Agent;
        zAnim = attackingZombie.ZAnim;
        playerTransform = attackingZombie.PlayerTransform;
    }
 
    public override void Update()
    {
        agent.SetDestination(playerTransform.position);
        zAnim.SetFloat(Speed, agent.velocity.magnitude);
        if (Vector3.SqrMagnitude(zombieTransform.position - playerTransform.position) < sqrAttackRadius)
        {
            agent.velocity = Vector3.zero;
            machine.SwitchState(attackingZombie.AttackState);
        }
        else if (Vector3.SqrMagnitude(zombieTransform.position - playerTransform.position) > sqrChaseRadius)
        {
            machine.SwitchState(attackingZombie.IdleState);
        }
    }
}
