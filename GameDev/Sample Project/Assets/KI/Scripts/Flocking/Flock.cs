using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    [SerializeField, Range(10, 250)] private int startingCount = 20;
    [SerializeField, Range(1f, 100f)] private float moveMulti = 10f, maxSpeed = 5f;
    [SerializeField, Range(1f, 10f)] private float neighbourRadius;
    [SerializeField, Range(0f, 1f)] private float avoidanceRadius = 0.5f;
    [SerializeField] private FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    [SerializeField] private FlockBehaviour behaviour;

    private float squareMaxSpeed;
   
    private const float agentDensity = 0.08f;

    private float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    private void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        
        squareAvoidanceRadius = neighbourRadius * avoidanceRadius * avoidanceRadius;

        for (int i = 0; i < startingCount; i++)
        {
            Quaternion targetRotation = Random.rotation;
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                transform.position + Random.insideUnitSphere * startingCount * agentDensity,
                targetRotation,
                transform
                );
            newAgent.name = "Agent " + i;
            agents.Add(newAgent);
        }

    }
    private void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObjects(agent);

            Vector3 move = behaviour.CalculateMove(agent, context, this);
            
            move *= moveMulti;
            if(move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighbourRadius);
        foreach (Collider col in contextColliders)
        {
            if (col != agent.AgentCollider)
            {
                context.Add(col.transform);
            }
        }
        return context;
    }
}
