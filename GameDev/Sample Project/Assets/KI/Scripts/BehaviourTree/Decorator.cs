using UnityEngine;

namespace BehaviourTree
{
    public class Decorator : Node
    {
        protected readonly Node child;

        public Decorator(Node child)
        {
            this.child = child;
        }
    }
}