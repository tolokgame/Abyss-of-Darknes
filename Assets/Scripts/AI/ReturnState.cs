using UnityEngine;

public class ReturnState : EnemyState
{
    public ReturnState(EnemyBrain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        // Просто перемикаємо назад у патруль
        stateMachine.ChangeState(enemy.PatrolState);
    }
}