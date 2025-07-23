using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

    public class Idle : Node
    {
        private float sqrChaseRadius;
        private AttackingBT owner;
        private Vector3 target;
        private int speed = Animator.StringToHash("Speed");

        public Idle(AttackingBT owner)
        {
            this.owner = owner;
            sqrChaseRadius = owner.ChaseRadius * owner.ChaseRadius;

            SetRandomDestination();  
        }
        
        public override NodeState Evaluate()
        {
            if(owner.Agent.velocity == Vector3.zero)
            {
                SetRandomDestination();
            }
                
            owner.Anim.SetFloat(speed, owner.Agent.velocity.magnitude);
            
            state = NodeState.Running;
            return state;
        }

        private void SetRandomDestination()
        {
            var zombiePos = owner.ZombieTransform.position;
            
            Vector3 playerPos = Random.insideUnitCircle * owner.IdleRadius;
            
            target = new Vector3(playerPos.x + zombiePos.x, zombiePos.y,
                                 playerPos.y + zombiePos.z);
            
            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 5, 1))
                owner.Agent.SetDestination(hit.position);
        }
    }
