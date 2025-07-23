using BehaviourTree;

public class Restock : Node
{
    private readonly VikingBT owner;

    public Restock(VikingBT owner)
    {
        this.owner = owner;
    }

    public override NodeState Evaluate()
    {
        owner.Resources.IronAmount = 50f;
        state = NodeState.Success;
        return state;
    }
}