namespace BehaviourTree
{
    public class Repeater : Decorator
    {
        private readonly int repeatCount;
        private int counter;

        public Repeater(int repeatCount, Node child) : base(child)
        {
            this.repeatCount = repeatCount;
        }

        public override NodeState Evaluate()
        {
            state = child.Evaluate();

            if (state == NodeState.Running)
                return state;
            if (counter < repeatCount)
            {
                counter++;
                return NodeState.Running;
            }

            counter = 0;
            return state;
        }
    }
}