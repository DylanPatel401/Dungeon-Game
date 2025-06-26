using System;

using System.Collections;

using System.Collections.Generic;

using System.Reflection;

using UnityEngine;

using UnityEngine.AI;





public class _FSM : MonoBehaviour

{

    private NavMeshAgent agent;



    public Transform[] waypoints;

    public Transform idlePoint;

    public Transform enemy;



    public float chaseRange = 6f;

    public float idleTimeThreshold = 10f;

    public float wanderTimeThreshold = 20f;



    private int currentWaypoint = 0;

    private float wanderTimer = 0f;

    private float idleTimer = 0f;

    private enum State { Idle, Wander, Chase }

    private State currentState = State.Wander;



    void Start()

    {

        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)

            agent.SetDestination(waypoints[0].position);

    }



    private void Update()

    {

        float enemyDistance = Vector3.Distance(transform.position, enemy.position);



        switch (currentState)

        {

            case State.Chase:

                if (enemyDistance > chaseRange)

                {

                    currentState = State.Wander;

                    wanderTimer = 0f;

                }

                else

                {

                    MoveTo(enemy);

                }

                break;



            case State.Wander:

                Wander();

                if (enemyDistance <= chaseRange)

                {

                    currentState = State.Chase;

                }



                wanderTimer += Time.deltaTime;



                if (wanderTimer >= wanderTimeThreshold)

                {

                    currentState = State.Idle;

                    idleTimer = 0f;

                }





                break;



            case State.Idle:

                MoveTo(idlePoint);



                if (enemyDistance <= chaseRange)

                {

                    currentState = State.Chase;

                }

                //if (Vector3.Distance(transform.position, idlePoint.position) < 0.5f)

                //{

                idleTimer += Time.deltaTime;





                if (idleTimer >= idleTimeThreshold)

                {

                    currentState = State.Wander;

                    wanderTimer = 0f;

                }

                //}

                break;

        }

    }



    private void Wander()

    {

        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)

        {

            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;

            agent.SetDestination(waypoints[currentWaypoint].position);

        }

    }





    private void MoveTo(Transform target)

    {

        agent.SetDestination(target.transform.position);

    }



}