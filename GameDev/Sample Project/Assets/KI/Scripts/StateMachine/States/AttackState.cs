using UnityEngine;
using UnityEngine.AI;

public class AttackState : State
{
    private float sqrAttackRadius;
    private Transform zombieTransform;
    private NavMeshAgent agent;
    private Animator zAnim;
    private AttackingZombie attackingZombie;
    private Transform playerTransform;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attacking = Animator.StringToHash("attacking");

    public AttackState(StateMachine machine) : base(machine) 
    {
        attackingZombie = machine.Owner as AttackingZombie;
    }

    public override void Start()
    {
        sqrAttackRadius = attackingZombie.AttackRadius * attackingZombie.AttackRadius;
        zombieTransform = attackingZombie.transform;
        agent = attackingZombie.Agent;
        zAnim = attackingZombie.ZAnim;
        playerTransform = attackingZombie.PlayerTransform;
    }

    public override void Update()
    {
        zAnim.SetTrigger(Attacking);
        agent.SetDestination(zombieTransform.position);
        zombieTransform.LookAt(new Vector3(playerTransform.position.x, zombieTransform.position.y, playerTransform.position.z));
        zAnim.SetFloat(Speed, agent.velocity.magnitude);
        if (Vector3.SqrMagnitude(zombieTransform.position - attackingZombie.PlayerTransform.position) > sqrAttackRadius)
        {
            machine.SwitchState(attackingZombie.ChaseState);
        }
    }
}
