using BehaviourTree;
using UnityEngine;

public class CheckforItem : Node
{
    private readonly MailmanBT owner;


    public CheckforItem(MailmanBT owner)
    {
        this.owner = owner;
    }

    public override NodeState Evaluate()
    {
        if (owner.ItemContainer.ItemList == null || owner.ItemContainer.ItemList.Count <= 0)
        {
            return state = NodeState.Failure;
        }
        else
        {
            return state = NodeState.Success;
        }
    }
}