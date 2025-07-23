using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Home")]
public class ComeHomeBehaviour : FlockBehaviour
{    
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        Vector3 direction = flock.transform.position - agent.transform.position;
        return direction;
    }
}
