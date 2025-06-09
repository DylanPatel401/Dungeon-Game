using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Settings")]
    public float chaseRange = 8f;
    public float playerCheckRate = 0.2f; // How often to check for player

    [Header("References")]
    public Transform[] patrolPoints;
    public Transform player;
    public Transform idlePoint;

    private NavMeshAgent agent;
    private IState currentState;
    private float lastPlayerCheckTime;

    // States
    public PatrolState patrolState { get; private set; }
    public ChaseState chaseState { get; private set; }
    public IdleState idleState { get; private set; }



    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        patrolState = new PatrolState(this, agent, patrolPoints);
        chaseState = new ChaseState(this, agent, player, chaseRange);
        idleState = new IdleState(this, agent, idlePoint);

        //TransitionToState(patrolState);

        TransitionToState(idleState);

    }

    //void InitializeStates()
    //{
    //    patrolState = new PatrolState(this, agent, patrolPoints);
    //    chaseState = new ChaseState(this, agent, player, chaseRange);
    //    idleState = new IdleState(this, agent, idlePoint);
    //}

    void Update()
    {
        // Check for player at fixed intervals (better performance)
        if (Time.time - lastPlayerCheckTime > playerCheckRate)
        {
            lastPlayerCheckTime = Time.time;
            CheckForPlayer();
        }

        currentState?.Execute();
    }

    void CheckForPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        bool shouldChase = distanceToPlayer <= chaseRange;
        bool isChasing = currentState == chaseState;


        if (shouldChase && !isChasing)
        {
            TransitionToState(chaseState);
        }
        else if (!shouldChase && isChasing)
        {
            // Return to previous state or idle when player is lost
            TransitionToState(idleState);
        }
    }

    //public void TransitionToState(IState newState)
    //{
    //    currentState?.Exit();
    //    currentState = newState;
    //    currentState?.Enter();
    //}

    public void TransitionToState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, chaseRange);
    //}
}







//public class EnemyAI : MonoBehaviour
//{
//    public Transform[] patrolPoints;
//    public Transform player;
//    public Transform idlePoint;
//    public float chaseRange = 5f;

//    private NavMeshAgent agent;
//    private IState currentState;

//    public PatrolState patrolState;
//    public ChaseState chaseState;
//    public IdleState idleState;


//    void Start()
//    {
//        agent = GetComponent<NavMeshAgent>();

//        patrolState = new PatrolState(this, agent, patrolPoints);
//        chaseState = new ChaseState(this, agent, player, chaseRange);
//        idleState = new IdleState(this, agent, idlePoint);

//        //TransitionToState(patrolState);

//        TransitionToState(idleState);


//    }

//    void Update()
//    {
//        currentState.Execute();



//    }




//    public void TransitionToState(IState newState)
//    {
//        if (currentState != null)
//            currentState.Exit();

//        currentState = newState;
//        currentState.Enter();
//    }
//}
