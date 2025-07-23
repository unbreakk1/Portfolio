using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        private int currentNode;
        public Sequence()
        {
        }

        public Sequence(List<Node> children) : base(children)
        {
        }

        public override NodeState Evaluate()
        {
            // foreach (Node node in children)
            // int index;
            for (; currentNode < children.Count; currentNode++)
            {
                Node node = children[currentNode];
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        currentNode = 0;
                        return state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        return NodeState.Running;
                    default:
                        state = NodeState.Success;
                        currentNode = 0;
                        return state;
                }
            }
            
            state = NodeState.Success;
            currentNode = 0;
            return state;
        }
    }
}