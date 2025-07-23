using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

public class ThiefBT : BTree
{
    [SerializeField] private Animator anim;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform itemSpawn;
    [SerializeField] private Transform thiefDen;

    public Animator Anim => anim;
    public NavMeshAgent Agent => agent;
    public Transform ItemSpawn => itemSpawn;
    public Transform ThiefDen => thiefDen;
    
    protected override Node SetupTree()
    {
        Node root = new Node();

        return root;
    }
}
