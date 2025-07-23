using BehaviourTree;

public class SetRestockTarget : Node
{
    private readonly VikingBT owner;

    public SetRestockTarget(VikingBT owner)
    {
        this.owner = owner;
    }

    public override NodeState Evaluate()
    {
        owner.Agent.SetDestination(owner.MineEntrance.position);
        owner.Anim.SetBool("isWorking", false);
        state = NodeState.Success;
        return state;
    }
}