using BehaviourTree;

public class ResourceCheck : Node
{
    private VikingBT owner;
    
      public ResourceCheck(VikingBT owner)
      {
          this.owner = owner;
      }
      
   public override NodeState Evaluate()
   {
       if (owner.Resources.IronAmount <= 0)
       {
           state = NodeState.Failure;
           return state;
       }
       
       if (owner.Resources.IronAmount > 0)
       {
           owner.Agent.SetDestination(owner.DebugWaypoint.position);
           state = NodeState.Success;
           return state;
       }

       state = NodeState.Failure;
       return state;
   }
  }
