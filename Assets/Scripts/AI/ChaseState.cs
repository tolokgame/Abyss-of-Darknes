using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyBrain enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Update()
    {
        // якщо Player був знищений Ч просто чекаЇмо,
        // поки EnemyBrain знайде нового п≥сл€ respawn.
        if (enemy.player == null)
            return;

        // якщо втратили гравц€ з пол€ зору Ч шукаЇмо
        if (!enemy.CanSeePlayer())
        {
            enemy.lastKnownPlayerPosition = enemy.player.position;
            stateMachine.ChangeState(enemy.SearchState);
            return;
        }

        // Ѕ≥жимо за гравцем
        enemy.agent.SetDestination(enemy.player.position);

        float distance = Vector3.Distance(
            enemy.transform.position,
            enemy.player.position
        );

        if (distance <= enemy.attackRange)
        {
            stateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void Enter()
    {
        base.Enter();

        // ѕов≥домл€Їмо менеджерам, що почалас€ погон€
        if (ChaseMusicManager.Instance != null)
            ChaseMusicManager.Instance.StartChase();

        if (ChaseCameraShakeManager.Instance != null)
            ChaseCameraShakeManager.Instance.StartChase();
    }

    public override void Exit()
    {
        base.Exit();

        // ѕов≥домл€Їмо менеджерам, що погон€ зак≥нчилас€
        if (ChaseMusicManager.Instance != null)
            ChaseMusicManager.Instance.StopChase();

        if (ChaseCameraShakeManager.Instance != null)
            ChaseCameraShakeManager.Instance.StopChase();
    }
}






/*using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyBrain enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Update()
    {
        // якщо Player був знищений Ч просто чекаЇмо,
        // поки EnemyBrain знайде нового п≥сл€ respawn.
        if (enemy.player == null)
            return;

        // якщо втратили гравц€ з пол€ зору Ч шукаЇмо
        if (!enemy.CanSeePlayer())
        {
            enemy.lastKnownPlayerPosition = enemy.player.position;
            stateMachine.ChangeState(enemy.SearchState);
            return;
        }

        // Ѕ≥жимо за гравцем
        enemy.agent.SetDestination(enemy.player.position);

        float distance = Vector3.Distance(
            enemy.transform.position,
            enemy.player.position
        );

        if (distance <= enemy.attackRange)
        {
            stateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void Enter()
    {
        base.Enter();
        // ѕов≥домл€Їмо менеджеру, що почалас€ погон€
        if (ChaseMusicManager.Instance != null)
            ChaseMusicManager.Instance.StartChase();
    }

    public override void Exit()
    {
        base.Exit();
        // ѕов≥домл€Їмо менеджеру, що погон€ зак≥нчилас€
        if (ChaseMusicManager.Instance != null)
            ChaseMusicManager.Instance.StopChase();
    }

}

*/






/*using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyBrain enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Update()
    {
        // якщо Player був знищений Ч просто чекаЇмо,
        // поки EnemyBrain знайде нового п≥сл€ respawn.
        if (enemy.player == null)
            return;

        // якщо втратили гравц€ з пол€ зору Ч шукаЇмо
        if (!enemy.CanSeePlayer())
        {
            enemy.lastKnownPlayerPosition = enemy.player.position;
            stateMachine.ChangeState(enemy.SearchState);
            return;
        }

        // Ѕ≥жимо за гравцем
        enemy.agent.SetDestination(enemy.player.position);

        float distance = Vector3.Distance(
            enemy.transform.position,
            enemy.player.position
        );

        if (distance <= enemy.attackRange)
        {
            stateMachine.ChangeState(enemy.AttackState);
        }
    }

    public override void Enter()
    {
    }
}
*/