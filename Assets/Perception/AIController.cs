
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AIState { Patrol, Investigate }

public class AIController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float viewDistance = 10f;
    public float viewAngle = 90f;
    public LayerMask playerLayer;
    public Transform eyes;
    public float waitTime = 2f;

    private int currentPoint = 0;
    private NavMeshAgent agent;
    private AIState currentState = AIState.Patrol;
    private Vector3 investigatePoint;
    private float waitTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextPoint();
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            Debug.Log("Player seen!");
            return;
        }

        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Investigate:
                Investigate();
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPoint();
        }
    }

    void Investigate()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer > waitTime)
            {
                currentState = AIState.Patrol;
                GoToNextPoint();
            }
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    public void HearNoise(Vector3 point)
    {
        investigatePoint = point;
        currentState = AIState.Investigate;
        agent.destination = investigatePoint;
        waitTimer = 0f;
    }

    bool CanSeePlayer()
    {
        Collider[] hits = Physics.OverlapSphere(eyes.position, viewDistance, playerLayer);
        foreach (var hit in hits)
        {
            Vector3 dir = (hit.transform.position - eyes.position).normalized;
            if (Vector3.Angle(eyes.forward, dir) < viewAngle / 2f)
            {
                if (!Physics.Linecast(eyes.position, hit.transform.position, ~playerLayer))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void OnDrawGizmosSelected()
    {
        if (eyes != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(eyes.position, viewDistance);

            Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * eyes.forward * viewDistance;
            Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * eyes.forward * viewDistance;
            Gizmos.DrawLine(eyes.position, eyes.position + left);
            Gizmos.DrawLine(eyes.position, eyes.position + right);
        }
    }
}
