using UnityEngine;

namespace BehaviourTree
{
    public abstract class BTree : MonoBehaviour
    {
        private Node root;

        protected virtual void Start()
        {
            root = SetupTree();
        }
        protected virtual void Update()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}