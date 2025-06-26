using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;

public class BT_Modular : MonoBehaviour
{
    // --- Public fields ---
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
}
