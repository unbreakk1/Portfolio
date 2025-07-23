using BehaviourTree;
using UnityEngine;

public class WaitTask : Decorator
{
    private readonly float waitTimer;
    private float waitCounter;


    public WaitTask(Node child, float waitTimer) : base(child)
    {
        this.waitTimer = waitTimer;
    }

    public override NodeState Evaluate()
    {
        if ((waitCounter += Time.deltaTime) <= waitTimer)
            // do wait stuff
            return state = NodeState.Running;

        state = child.Evaluate();
        if (state != NodeState.Running) 
            waitCounter = 0;
        
        return state;
    }
}