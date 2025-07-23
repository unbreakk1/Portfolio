using BehaviourTree;
using UnityEngine.AI;

public class MoveToTask : Node
{

    private NavMeshAgent agent;
    

    public MoveToTask(NavMeshAgent agent)
    {
        this.agent = agent;
    }
    public override NodeState Evaluate()
    {
        if (agent.pathPending)
        {
            return NodeState.Running;
        }
        
        if (agent.GetPathRemainingDistance() <= agent.stoppingDistance)
        {
            state = NodeState.Success;
            return state;
        }
        
        state = NodeState.Running;
        return state;

    }
}
