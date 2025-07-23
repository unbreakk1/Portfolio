using UnityEngine;
using BehaviourTree;

public class ChaseRangeCheck : Node
{
    private AttackingBT owner;
    private float sqrChaseRadius;
    
    public ChaseRangeCheck(AttackingBT owner)
    {
        this.owner = owner;
        sqrChaseRadius = owner.ChaseRadius * owner.ChaseRadius;
    }

    public override NodeState Evaluate()
    {
        if (Vector3.SqrMagnitude(owner.ZombieTransform.position - owner.PlayerTransform.position) < sqrChaseRadius)
        {
            state = NodeState.Success;
            return state;
        }
        else
        {
            state = NodeState.Failure;
            return state; 
        }
    }
}
