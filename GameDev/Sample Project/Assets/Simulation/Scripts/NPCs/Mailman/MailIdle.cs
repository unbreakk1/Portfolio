using BehaviourTree;

public class MailIdle : Decorator
{
    private readonly MailmanBT owner;
    private bool hasWaypoint;

    private int waypointIndex;

    public MailIdle(Node child, MailmanBT owner) : base(child)
    {
        this.owner = owner;
    }

    public override NodeState Evaluate()
    {
        if (hasWaypoint == false)
        {
            waypointIndex = (waypointIndex + 1) % owner.Waypoints.Length;
            owner.Agent.SetDestination(owner.Waypoints[waypointIndex].position);

            hasWaypoint = true;
        }

        state = child.Evaluate();
        if (state == NodeState.Running) 
            return state;

        hasWaypoint = false;
        return state;
    }
}