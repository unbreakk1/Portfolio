using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingZombie : Zombie
{
    [SerializeField] private float evadeRadius;
    private FleeingIdle idleState;
    private FleeState fleeState;

    public FleeingIdle IdleState => idleState;
    public FleeState FleeState => fleeState;
    public float EvadeRadius => evadeRadius;

    protected override void InitializeState()
    {
        idleState = new FleeingIdle(brain);
        fleeState = new FleeState(brain);
        brain.Initialize(idleState);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, evadeRadius);
    }
}
