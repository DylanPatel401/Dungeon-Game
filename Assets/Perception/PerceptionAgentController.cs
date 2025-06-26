using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Perception_AgentController : MonoBehaviour
{
    [Header("AI Stats")]
    public float currentHealth = 100f;

    [Header("AI Configuration")]
    public float chaseRange = 6f;
    public float idleTimeThreshold = 10f;
    public float wanderTimeThreshold = 20f;

    [Header("AI References")]
    public Transform[] waypoints;
    public Transform idlePoint;
    public Transform enemy;

    private NavMeshAgent agent;
    private Node root;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckHealthNode(this, 25f),
                new FleeNode2(agent, enemy)
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

    // ---------- CONDITION NODE: ENEMY IN RANGE ----------
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

    // ---------- CONDITION NODE: LOW HEALTH ----------
    private class CheckHealthNode : Node
    {
        private Perception_AgentController bt;
        private float healthThreshold;

        public CheckHealthNode(Perception_AgentController bt, float threshold)
        {
            this.bt = bt;
            this.healthThreshold = threshold;
        }

        public override NodeStatus Evaluate()
        {
            return bt.currentHealth <= healthThreshold ? NodeStatus.SUCCESS : NodeStatus.FAILURE;
        }
    }

    public void HearNoise(Vector3 point)
    {
        Debug.Log($"{gameObject.name} heard a noise at {point}");

        // Example response: navigate to the sound location
        GetComponent<UnityEngine.AI.NavMeshAgent>().SetDestination(point);

        // OPTIONAL: If you use a BT or FSM, switch state here
        // e.g., state = AIState.Investigate;
    }

}
