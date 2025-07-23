using BehaviourTree;
using UnityEngine;

public class AttackRangeCheck : Node
{
   private AttackingBT owner;
   private float sqrAttackRadius;

   public AttackRangeCheck(AttackingBT owner)
   {
      this.owner = owner;
      sqrAttackRadius = owner.AttackRadius * owner.AttackRadius;
   }   
   public override NodeState Evaluate()
   {
      if (Vector3.SqrMagnitude(owner.ZombieTransform.position - owner.PlayerTransform.position) < sqrAttackRadius)
      {
         state = NodeState.Success;
         return state;
      } 
      
      state = NodeState.Failure; 
      return state;
   }
}
