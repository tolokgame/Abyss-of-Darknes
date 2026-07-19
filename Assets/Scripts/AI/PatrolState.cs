using UnityEngine;

public class PatrolState : EnemyState
{
    private bool isWaiting;
    private float waitTimer;

    // Ќова зм≥нна: затримка на активац≥ю зору (наприклад, 1.5 секунди)
    private float visionActivationTimer;

    public PatrolState(EnemyBrain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        isWaiting = false;

        // ƒаЇмо ворогов≥ 1.5 секунди побути "сл≥пим" при вход≥ в патруль
        visionActivationTimer = 1.5f;

        GoToNextWaypoint();
    }

    public override void Update()
    {
        // «меншуЇмо таймер затримки зору
        if (visionActivationTimer > 0)
        {
            visionActivationTimer -= Time.deltaTime;
        }
        // ¬орог почне бачити гравц€ “≤Ћ№ » п≥сл€ того, €к таймер зак≥нчитьс€
        else if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(enemy.ChaseState);
            return;
        }

        // --- “во€ стара лог≥ка патрулюванн€ ---
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = Random.Range(enemy.minPatrolWaitTime, enemy.maxPatrolWaitTime);
            }
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                GoToNextWaypoint();
            }
        }
    }

    private void GoToNextWaypoint()
    {
        if (enemy.patrolWaypoints == null || enemy.patrolWaypoints.Count == 0) return;
        int randomIndex = Random.Range(0, enemy.patrolWaypoints.Count);
        if (enemy.patrolWaypoints[randomIndex] != null)
        {
            enemy.agent.SetDestination(enemy.patrolWaypoints[randomIndex].position);
        }
    }
}

/*using UnityEngine;

public class PatrolState : EnemyState
{
    private bool isWaiting;
    private float waitTimer;

    public PatrolState(EnemyBrain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        isWaiting = false;
        GoToNextWaypoint();

        enemy.enemyRenderer.material.color = Color.black;
    }

    public override void Update()
    {
        // якщо побачив гравц€ -> перемикаЇмос€ на погоню
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(enemy.ChaseState);
            return;
        }

        // якщо д≥йшов до точки патрулюванн€
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = Random.Range(enemy.minPatrolWaitTime, enemy.maxPatrolWaitTime);
            }
        }

        // Ћог≥ка оч≥куванн€ на точц≥
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                GoToNextWaypoint();
            }
        }
    }

    private void GoToNextWaypoint()
    {
        if (enemy.patrolWaypoints == null || enemy.patrolWaypoints.Count == 0) return;

        int randomIndex = Random.Range(0, enemy.patrolWaypoints.Count);
        if (enemy.patrolWaypoints[randomIndex] != null)
        {
            enemy.agent.SetDestination(enemy.patrolWaypoints[randomIndex].position);
        }
    }
}*/