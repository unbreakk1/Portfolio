using BehaviourTree;


public class IdleTask : Node
 {
     private VikingBT owner;
     
     public IdleTask(VikingBT owner)
     {
         this.owner = owner;
     }
     
     public override NodeState Evaluate()
     {
         state = NodeState.Running;
         return state;
     }
 }
