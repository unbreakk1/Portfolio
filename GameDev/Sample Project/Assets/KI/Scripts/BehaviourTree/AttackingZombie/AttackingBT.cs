using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class AttackingBT : BTree
{
    [SerializeField] private int idleRadius;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform zombieTransform;
    [SerializeField] private Transform playerTransform;
    
    public float ChaseRadius => chaseRadius;
    public float AttackRadius => attackRadius;
    public Animator Anim => anim;
    public NavMeshAgent Agent => agent;
    public Transform ZombieTransform => zombieTransform;
    public int IdleRadius => idleRadius;
    public Transform PlayerTransform => playerTransform;
    
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new AttackRangeCheck(this),
                new AttackTask(this, new AttackRangeCheck(this))
            }),
            new Sequence(new List<Node>
            {
                new ChaseRangeCheck(this),
                new ChaseTask(this, new ChaseRangeCheck(this))
            }),
            new Idle(this),
        });
        return root;
    }
}
