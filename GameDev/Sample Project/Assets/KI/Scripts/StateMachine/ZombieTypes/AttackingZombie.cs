using UnityEngine;

public class AttackingZombie : Zombie
{
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    private ChaseState chaseState;
    private IdleState idleState;
    private AttackState attackState;

    public ChaseState ChaseState => chaseState;
    public IdleState IdleState => idleState;
    public AttackState AttackState => attackState;

    public float ChaseRadius => chaseRadius;
    public float AttackRadius => attackRadius;

    protected override void InitializeState()
    {
        idleState = new IdleState(brain);
        chaseState = new ChaseState(brain);
        attackState = new AttackState(brain);
        brain.Initialize(idleState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}
