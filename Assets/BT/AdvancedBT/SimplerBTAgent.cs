using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobustBTAgent : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform idlePoint;
    public Transform[] wanderPoints;

    [Header("Behavior Settings")]
    public float chaseSpeed = 5f;
    public float wanderSpeed = 2f;
    public float returnSpeed = 3f;
    public float detectionRange = 5f;
    public float idleWaitTime = 3f;
    public float wanderDuration = 10f;

    private enum AgentState { GoingToIdle, Idle, Wandering, Chasing }
    private AgentState _currentState;

    private int _currentWanderIndex = 0;
    private float _stateTimer = 0f;
    private Vector3 _lastKnownPlayerPosition;

    void Start()
    {
        _currentState = AgentState.GoingToIdle;
        Debug.Log("Initial State: GoingToIdle");
    }

    void Update()
    {
        if (player == null || idlePoint == null || wanderPoints == null || wanderPoints.Length == 0)
        {
            Debug.LogError("Missing critical references!");
            return;
        }

        bool playerInRange = Vector3.Distance(transform.position, player.position) < detectionRange;

        // Highest priority: Chase player if detected
        if (playerInRange)
        {
            if (_currentState != AgentState.Chasing)
            {
                ChangeState(AgentState.Chasing);
            }
            ChasePlayer();
            return;
        }

        // State machine
        switch (_currentState)
        {
            case AgentState.Chasing:
                // Lost sight of player
                ChangeState(AgentState.GoingToIdle);
                break;

            case AgentState.GoingToIdle:
                MoveTo(idlePoint.position, returnSpeed);
                if (AtPosition(idlePoint.position))
                {
                    ChangeState(AgentState.Idle);
                }
                break;

            case AgentState.Idle:
                _stateTimer += Time.deltaTime;
                if (_stateTimer >= idleWaitTime)
                {
                    ChangeState(AgentState.Wandering);
                }
                break;

            case AgentState.Wandering:
                _stateTimer += Time.deltaTime;

                // Check if wander duration expired
                if (_stateTimer >= wanderDuration)
                {
                    ChangeState(AgentState.GoingToIdle);
                    break;
                }

                // Continue wandering
                Wander();
                break;
        }
    }

    void ChasePlayer()
    {
        _lastKnownPlayerPosition = player.position;
        MoveTo(_lastKnownPlayerPosition, chaseSpeed);
        Debug.DrawLine(transform.position, _lastKnownPlayerPosition, Color.red);
    }

    void Wander()
    {
        Transform target = wanderPoints[_currentWanderIndex];
        MoveTo(target.position, wanderSpeed);

        if (AtPosition(target.position))
        {
            _currentWanderIndex = (_currentWanderIndex + 1) % wanderPoints.Length;
            Debug.Log($"Reached waypoint. Next: {_currentWanderIndex}");
        }
    }

    void MoveTo(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    bool AtPosition(Vector3 target)
    {
        return Vector3.Distance(transform.position, target) < 0.1f;
    }

    void ChangeState(AgentState newState)
    {
        if (_currentState == newState) return;

        Debug.Log($"State change: {_currentState} -> {newState}");

        // Exit current state
        switch (_currentState)
        {
            case AgentState.Wandering:
                _currentWanderIndex = 0;
                break;
        }

        // Enter new state
        switch (newState)
        {
            case AgentState.Idle:
            case AgentState.Wandering:
                _stateTimer = 0f;
                break;
        }

        _currentState = newState;
    }

    void OnDrawGizmos()
    {
        // Draw detection range
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, detectionRange);

        // Draw wander path
        if (wanderPoints != null && wanderPoints.Length > 1)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < wanderPoints.Length; i++)
            {
                if (wanderPoints[i] == null) continue;
                Gizmos.DrawSphere(wanderPoints[i].position, 0.3f);
                int next = (i + 1) % wanderPoints.Length;
                if (wanderPoints[next] != null)
                {
                    Gizmos.DrawLine(wanderPoints[i].position, wanderPoints[next].position);
                }
            }
        }
    }
}