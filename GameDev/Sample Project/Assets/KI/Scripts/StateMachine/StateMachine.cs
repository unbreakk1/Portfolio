using UnityEngine;
using UnityEngine.AI;
public class StateMachine
{
    private State currentState;
    private Zombie owner;

    public Zombie Owner => owner;

    public StateMachine(Zombie owner) 
    {
        this.owner = owner;
    }

    public void Initialize(State state)
    {
        currentState = state;
        currentState.Start();
    }

    public void Update()
    {
        currentState.Update();
    }

    public void SwitchState(State newState)
    {
        currentState.Stop();
        currentState = newState;
        currentState.Start();
    }
}
