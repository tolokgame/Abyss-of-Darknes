using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyBrain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Update()
    {
        // якщо втратили гравц€ з пол€ зору, то виходить на пошуки
        if (!enemy.CanSeePlayer())
        {
            enemy.lastKnownPlayerPosition = enemy.player.position;
            stateMachine.ChangeState(enemy.SearchState);
            return;

        }

        // ѕост≥йно б≥жимо за гравцем
        if (enemy.player != null)
        {
            enemy.agent.SetDestination(enemy.player.position);

        }
    }

    public override void Enter()
    {
       
    }
}