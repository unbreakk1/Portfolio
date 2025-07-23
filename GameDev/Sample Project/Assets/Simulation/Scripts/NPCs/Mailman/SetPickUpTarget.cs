using BehaviourTree;

public class SetPickUpTarget : Decorator
{
    private MailmanBT owner;
    private bool hasPath;

    public SetPickUpTarget(MailmanBT owner, Node child) : base(child)
    {
        this.owner = owner;
        hasPath = false;
    }
    
    public override NodeState Evaluate()
    {
        if (hasPath == false)
            owner.Agent.SetDestination(owner.ItemSpawn.position);
        
        state = child.Evaluate();
        
        hasPath = state == NodeState.Running;
        
        return state;
    }
}
