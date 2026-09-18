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
    public List<Transform> patrolWaypoints;
    public float minPatrolWaitTime = 1f;
    public float maxPatrolWaitTime = 4f;

    [Header("Detection Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float eyeHeight = 1.5f;

    [Header("Search Settings")]
    public float searchDuration = 5f;

    [HideInInspector] public Vector3 lastKnownPlayerPosition;

    private PlayerState playerState;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        PatrolState = new PatrolState(this, StateMachine);
        ChaseState = new ChaseState(this, StateMachine);
        SearchState = new SearchState(this, StateMachine);
        ReturnState = new ReturnState(this, StateMachine);
        AttackState = new AttackState(this, StateMachine);
    }

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (enemyRenderer == null)
            enemyRenderer = GetComponent<Renderer>();

        FindPlayer();

        // Стартуємо з патрулювання
        StateMachine.Initialize(PatrolState);
    }

    private void Update()
    {
        // Якщо старий Player був знищений,
        // шукаємо нового після respawn.
        if (player == null)
        {
            FindPlayer();
        }

        // Оновлюємо поточний стан
        StateMachine.CurrentState.Update();

        // Колір ворога залежно від стану
        if (StateMachine.CurrentState == ChaseState)
        {
            enemyRenderer.material.color = Color.red;
        }
        else if (StateMachine.CurrentState == SearchState)
        {
            enemyRenderer.material.color = Color.yellow;
        }
        else
        {
            enemyRenderer.material.color = Color.green;
        }
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            player = null;
            playerState = null;
            return;
        }

        player = playerObject.transform;
        playerState = playerObject.GetComponent<PlayerState>();
    }

    public bool CanSeePlayer()
    {
        if (player == null)
            return false;

        // Якщо гравець сховався — ворог його не бачить
        if (playerState != null && playerState.IsHidden)
            return false;

        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        Vector3 targetPosition = player.position + Vector3.up * eyeHeight;

        // Відстань до гравця
        float distance = Vector3.Distance(
            eyePosition,
            targetPosition
        );

        // Перевірка радіуса
        if (distance <= viewRadius)
        {
            // Напрямок до гравця
            Vector3 direction = (
                targetPosition - eyePosition
            ).normalized;

            // Перевірка кута огляду
            if (Vector3.Angle(
                transform.forward,
                direction
            ) < viewAngle / 2f)
            {
                // Перевірка стіни між ворогом і гравцем
                if (!Physics.Raycast(
                    eyePosition,
                    direction,
                    distance,
                    obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        // Радіус огляду
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            viewRadius
        );

        // Лінія до гравця
        if (player != null)
        {
            Vector3 eyePosition =
                transform.position +
                Vector3.up * eyeHeight;

            Vector3 targetPosition =
                player.position +
                Vector3.up * eyeHeight;

            Gizmos.color = CanSeePlayer()
                ? Color.green
                : Color.red;

            Gizmos.DrawLine(
                eyePosition,
                targetPosition
            );
        }
    }
}









/*using System.Collections.Generic;
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
    public List<Transform> patrolWaypoints;
    public float minPatrolWaitTime = 1f;
    public float maxPatrolWaitTime = 4f;

    [Header("Detection Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)] public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float eyeHeight = 1.5f;

    [Header("Search Settings")]
    public float searchDuration = 5f;

    [HideInInspector] public Vector3 lastKnownPlayerPosition;

    private PlayerState playerState;

    private void Awake()
    {
        StateMachine = new EnemyStateMachine();

        PatrolState = new PatrolState(this, StateMachine);
        ChaseState = new ChaseState(this, StateMachine);
        SearchState = new SearchState(this, StateMachine);
        ReturnState = new ReturnState(this, StateMachine);
        AttackState = new AttackState(this, StateMachine);
    }

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        if (enemyRenderer == null)
            enemyRenderer = GetComponent<Renderer>();

        FindPlayer();

        // Стартуємо з патрулювання
        StateMachine.Initialize(PatrolState);
    }

    private void FindPlayer()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject == null)
        {
            player = null;
            playerState = null;
            return;
        }

        player = playerObject.transform;
        playerState = playerObject.GetComponent<PlayerState>();
    }

    private void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }

        StateMachine.CurrentState.Update();

        if (StateMachine.CurrentState == ChaseState)
        {
            enemyRenderer.material.color = Color.red;
        }
        else if (StateMachine.CurrentState == SearchState)
        {
            enemyRenderer.material.color = Color.yellow;
        }
        else
        {
            enemyRenderer.material.color = Color.green;
        }
    }

    public bool CanSeePlayer()
    {
        if (player == null)
            return false;

        // Якщо гравець сховався — ворог його не бачить
        if (playerState != null && playerState.IsHidden)
            return false;

        Vector3 eyePosition = transform.position + Vector3.up * eyeHeight;
        Vector3 targetPosition = player.position + Vector3.up * eyeHeight;

        // Відстань до гравця
        float distance = Vector3.Distance(
            eyePosition,
            targetPosition
        );

        // Перевірка радіуса
        if (distance <= viewRadius)
        {
            // Напрямок до гравця
            Vector3 direction = (
                targetPosition - eyePosition
            ).normalized;

            // Перевірка кута огляду
            if (Vector3.Angle(
                transform.forward,
                direction
            ) < viewAngle / 2f)
            {
                // Перевірка стіни між ворогом і гравцем
                if (!Physics.Raycast(
                    eyePosition,
                    direction,
                    distance,
                    obstacleMask))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        // Радіус огляду
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            viewRadius
        );

        // Лінія до гравця
        if (player != null)
        {
            Vector3 eyePosition =
                transform.position +
                Vector3.up * eyeHeight;

            Vector3 targetPosition =
                player.position +
                Vector3.up * eyeHeight;

            Gizmos.color = CanSeePlayer()
                ? Color.green
                : Color.red;

            Gizmos.DrawLine(
                eyePosition,
                targetPosition
            );
        }
    }
} 
*/




