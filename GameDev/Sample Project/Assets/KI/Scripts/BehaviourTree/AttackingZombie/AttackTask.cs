using BehaviourTree;
using UnityEngine;

public class AttackTask : Decorator
{
    
    private float sqrAttackRadius;
    private AttackingBT owner;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attacking = Animator.StringToHash("attacking");

    public AttackTask(AttackingBT owner,Node child) : base(child)
    {
        this.owner = owner;
        sqrAttackRadius = this.owner.AttackRadius * this.owner.AttackRadius;
    }

    public override NodeState Evaluate()
    {
        owner.Anim.SetTrigger(Attacking);
        owner.Agent.SetDestination(owner.ZombieTransform.position);
        owner.ZombieTransform.LookAt(new Vector3(owner.PlayerTransform.position.x, owner.ZombieTransform.position.y, 
                                                                      owner.PlayerTransform.position.z));
        owner.Anim.SetFloat(Speed, owner.Agent.velocity.magnitude);

        state = child.Evaluate();
        return state;
    }
}
