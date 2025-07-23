using BehaviourTree;
using UnityEngine;

public class WorkTask : Decorator
{
    private readonly int isWorking = Animator.StringToHash("isWorking");
    private readonly VikingBT owner;

    public WorkTask(VikingBT owner, Node child) : base(child)
    {
        this.owner = owner;
    }

    public override NodeState Evaluate()
    {
        owner.transform.LookAt(new Vector3(owner.WorkPlaceTransform.position.x, owner.transform.position.y,
            owner.WorkPlaceTransform.position.z));
        owner.Anim.SetBool(isWorking, true);

        state = child.Evaluate();
        return state;
    }
}