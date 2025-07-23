using System.Collections.Generic;
using BehaviourTree;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class VikingBT : BTree
{
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField, Expandable] private ResourceObject resources;
    [SerializeField] private Transform mineEntrance;
    [SerializeField] private Transform debugWaypoint;
    [SerializeField] private Transform workPlaceTransform;

    public Animator Anim => anim;
    public NavMeshAgent Agent => agent;
    public ResourceObject Resources => resources;
    public Transform MineEntrance => mineEntrance;
    public Transform DebugWaypoint => debugWaypoint;
    public Transform WorkPlaceTransform => workPlaceTransform;

    private Node root;

    protected override Node SetupTree()
    {
        root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new ResourceCheck(this),
                new MoveToTask(agent),
                new WorkTask(this, new ResourceCheck(this)),
            }),
            new Sequence(new List<Node>
            {
                new SetRestockTarget(this),
                new MoveToTask(agent),
                new Restock(this)
            })
        });

        return root;
    }
}