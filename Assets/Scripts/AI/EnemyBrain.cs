using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour
{
    public EnemyStateMachine StateMachine { get; private set; }

    // Наші стани
    public PatrolState PatrolState { get; private set; }
    public ChaseState ChaseState { get; private set; }
    public SearchState SearchState { get; private set; }
    public ReturnState ReturnState { get; private set; }

    public AttackState AttackState { get; private set; }

    [Header("Attack Settings")]
    public float attackRange = 5f;
    public float attackDamage = 1f;
    public float attackCooldown = 1f;

    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public Renderer enemyRenderer;

    [Header("Maze Patrol Settings")]
    // Список точок лабіринту, які ми налаштуємо в інспекторі Unity
    public List<Transform> patrolWaypoints;
    public float minPatrolWaitTime = 1f;
    public float maxPatrolWaitTime = 4f;

    [Header("Detection Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;

    [Header("Search Settings")]
    public float searchDuration = 5f;

    [HideInInspector] public Vector3 lastKnownPlayerPosition;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        // Ініціалізуємо всі 4 стани і передаємо їм цей мозок (this) та перемикач
        PatrolState = new PatrolState(this, StateMachine);
        ChaseState = new ChaseState(this, StateMachine);
        SearchState = new SearchState(this, StateMachine);
        ReturnState = new ReturnState(this, StateMachine);
        AttackState = new AttackState(this, StateMachine);
    }

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();


        if (enemyRenderer == null) enemyRenderer = GetComponent<Renderer>();

        // Стартуємо з патрулювання
        StateMachine.Initialize(PatrolState);
    }

    private void Update()
    {
        // 1. Спочатку оновлюємо логіку поточного стану
        StateMachine.CurrentState.Update();

        // 2. А тепер ЗАВЖДИ і залізобетонно керуємо кольором залежно від стану
        if (StateMachine.CurrentState == ChaseState)
        {
            enemyRenderer.material.color = Color.red; // Погоня -> Червоний
        }
        else if (StateMachine.CurrentState == SearchState)
        {
            enemyRenderer.material.color = Color.yellow; // Пошук -> Жовтий
        }
        else
        {
            enemyRenderer.material.color = Color.green; // У всіх інших випадках (Патруль) -> Зелений
        }
    }

    // Метод перевірки зору, який тепер використовують стани
    public bool CanSeePlayer()
    {
        if (player == null) return false;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= viewRadius)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, direction) < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position + Vector3.up, direction, distance, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}