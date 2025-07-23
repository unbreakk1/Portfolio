using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    
    protected StateMachine machine;

    public State(StateMachine machine)
    {
        this.machine = machine;
    }

    public virtual void Start() { }
    public abstract void Update();
    public virtual void Stop() { }

}
