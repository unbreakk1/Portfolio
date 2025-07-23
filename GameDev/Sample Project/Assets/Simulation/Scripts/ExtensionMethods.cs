using UnityEngine;
using UnityEngine.AI;

public static class ExtensionMethods
{
    public static float GetPathRemainingDistance(this NavMeshAgent navMeshAgent)
    {
        float distance = 0;
        Vector3[] corners = navMeshAgent.path.corners;

        if (corners.Length > 2)
        {
            for (int i = 1; i < corners.Length; i++)
            {
                Vector2 previous = new Vector2(corners[i - 1].x, corners[i - 1].z);
                Vector2 current = new Vector2(corners[i].x, corners[i].z);

                distance += Vector2.Distance(previous, current);
            }
        }
        else 
        {
            distance = navMeshAgent.remainingDistance;
        }

        return distance;
    }
}