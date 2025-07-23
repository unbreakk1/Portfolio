using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

public class MailmanBT : BTree
{
    [SerializeField] private float waitDuration;
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform itemSpawn;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private ItemSpawn itemContainer;
    [SerializeField] private List<GameObject> mailInventory = new();
    [SerializeField] private Transform startPosition;

   
    public Animator Anim => anim;
    public NavMeshAgent Agent => agent;
    public Transform ItemSpawn => itemSpawn;
    public Transform[] Waypoints => waypoints;
    public ItemSpawn ItemContainer => itemContainer;
    public List<GameObject> MailInventory => mailInventory;
    public Transform StartPosition => startPosition;
    
    protected override Node SetupTree()
    {
       Sequence deliverSequence = new Sequence(new List<Node>
       {
           new CheckforItem(this),
           new SetPickUpTarget(this, new MoveToTask(agent)),
           new PickUpItem(this, new MoveToTask(agent)),
           new DeliverItem(this, new MoveToTask(agent))
       });
       
       Node mailIdle = new MailIdle(new MoveToTask(agent), this);
       Node repeater = new Repeater(Waypoints.Length, mailIdle);
       Node root = new WaitTask(new Selector(new List<Node>
       {
           deliverSequence,
           repeater,
       }), waitDuration);
       
        return root;
    }
}