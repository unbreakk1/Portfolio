using BehaviourTree;
using UnityEngine;

public class ChaseTask : Decorator
{
    private AttackingBT owner;
    private int speed = Animator.StringToHash("Speed");


    public ChaseTask(AttackingBT owner,Node child) : base(child)
    {
        this.owner = owner;
    }
    
    public override NodeState Evaluate()
    {
        owner.Agent.SetDestination(owner.PlayerTransform.position);
        owner.Anim.SetFloat(speed, owner.Agent.velocity.magnitude);

        state = child.Evaluate();
        return state;
    }
}
