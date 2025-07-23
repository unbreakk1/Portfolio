using BehaviourTree;
using UnityEngine;

public class DeliverItem : Decorator
{
    private bool hasPath;
    private Transform waypoint;
    private MailmanBT owner;

    public DeliverItem(MailmanBT owner, Node child) : base(child)
    {
        this.owner = owner;
        hasPath = false;
    }

    public override NodeState Evaluate()
    {
        if (owner.MailInventory is { Count: > 0 })
        {
            var item = owner.MailInventory[0];
            owner.MailInventory.Remove(item);
        }

        if (hasPath == false)
        {
            owner.Agent.SetDestination(owner.StartPosition.position);
        }

        Debug.Log("I run to Start");

        state = child.Evaluate();

        hasPath = state == NodeState.Running;

        return state;
    }
}