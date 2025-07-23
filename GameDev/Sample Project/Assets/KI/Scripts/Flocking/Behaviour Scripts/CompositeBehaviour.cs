using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{
    [SerializeField] private FlockBehaviour[] flockBehaves;
    [SerializeField] private float[] weights;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(weights.Length != flockBehaves.Length)
        {
            Debug.LogError("Data mismatch in" + name, this);
            return Vector3.zero;
        }
        //set up Move()
        Vector3 move = Vector3.zero;

        // iterate behaviours and create partialMove as "middleman"
        for (int i = 0; i < flockBehaves.Length; i++)
        {
            Vector3 partialMove = flockBehaves[i].CalculateMove(agent, context, flock) * weights[i];

            if(partialMove != Vector3.zero)
            {
                if(partialMove.sqrMagnitude > weights[i] * weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }
                move += partialMove;
            }
        }

        return move;
    }
}
