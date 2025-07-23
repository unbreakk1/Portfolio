using BehaviourTree;
using UnityEngine;


public class PickUpItem : Decorator
{
    private MailmanBT owner;
    private Transform wayPoint;
    private bool hasPath;

    public PickUpItem(MailmanBT owner, Node child) : base(child)
    {
        this.owner = owner;
    }

    public override NodeState Evaluate()
    {
        if (owner.ItemContainer.ItemList.Count > 0)
        {
            var item = owner.ItemContainer.ItemList[0];

            owner.MailInventory.Add(item);
            item.gameObject.SetActive(false);
            owner.ItemContainer.ItemList.Remove(item);
        }

        foreach (var mail in owner.MailInventory)
        {
            var itemScript = mail.GetComponent<ItemScript>();
            wayPoint = itemScript.Destinations[Random.Range(0, itemScript.Destinations.Length)];
        }

        if (hasPath == false)
            owner.Agent.SetDestination(wayPoint.position);

        state = child.Evaluate();

        hasPath = state == NodeState.Running;

        return state;
    }
}