using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using TreeEditor;

public class BT_Modular_With_Flee : MonoBehaviour
{
    // --- Public fields ---
    //-----------------------------------------------
    // Adding AI Stats for Flee action
    [Header("AI Stats")]
    public float currentHealth = 100f; // Add health here
    //-----------------------------------------------


    [Header("AI Configuration")]
    public float chaseRange = 6f;
    public float idleTimeThreshold = 10f;
    public float wanderTimeThreshold = 20f;

    [Header("AI References")]
    public Transform[] waypoints;
    public Transform idlePoint;
    public Transform enemy;

    // --- Private State Variables ---
    private NavMeshAgent agent;
    private Node root;

    // --- Unity Methods ---
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();


        root = new Selector(new List<Node>
        {
             // HIGHEST PRIORITY: Flee if health is low
            new Sequence(new List<Node>
            {
                // Pass 'this' (the BehaviorTree instance) to the condition node
                new CheckHealthNode(this, 25f),
                new FleeNode(agent, enemy)
            }),

            new Sequence(new List<Node>
            {
                new CheckEnemyInChaseRange(transform, enemy, chaseRange),
                new ChaseNode(agent, enemy)
            }),
            new PatrolNode(agent, waypoints, idlePoint, idleTimeThreshold, wanderTimeThreshold)
        });


    }

    void Update()
    {
        root.Evaluate();
    }

    // --- Condition Node ---
    /// <summary>
    /// Condition node to check if the enemy is within chase range.
    /// </summary>
    private class CheckEnemyInChaseRange : Node
    {
        private Transform self;
        private Transform enemy;
        private float chaseRange;

        public CheckEnemyInChaseRange(Transform self, Transform enemy, float chaseRange)
        {
            this.self = self;
            this.enemy = enemy;
            this.chaseRange = chaseRange;
        }

        public override NodeStatus Evaluate()
        {
            float distance = Vector3.Distance(self.position, enemy.position);
            return distance <= chaseRange ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }


    // --- Condition Node ---
    /// <summary>
    /// Condition node to check if the AI's health is low.
    /// </summary>
    private class CheckHealthNode : Node
    {
        private BT_Modular_With_Flee bt;
        private float healthThreshold;

        // The constructor now accepts the BehaviorTree component
        public CheckHealthNode(BT_Modular_With_Flee behaviorTree, float threshold)
        {
            this.bt = behaviorTree;
            this.healthThreshold = threshold;
        }

        public override NodeStatus Evaluate()
        {
            // Check the health from the BehaviorTree instance
            return bt.currentHealth <= healthThreshold ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }
}


/// <summary>
/// Action node for fleeing from the enemy.
/// </summary>
public class FleeNode : Node
{
    private NavMeshAgent agent;
    private Transform enemy;

    public FleeNode(NavMeshAgent agent, Transform enemy)
    {
        this.agent = agent;
        this.enemy = enemy;
    }

    public override NodeStatus Evaluate()
    {
        // Calculate a point away from the enemy
        Vector3 runTo = agent.transform.position + (agent.transform.position - enemy.position);
        agent.SetDestination(runTo);
        return NodeStatus.RUNNING;
    }
}
