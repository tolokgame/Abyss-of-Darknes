using UnityEngine;

public class ReturnState : EnemyState
{
    public ReturnState(EnemyBrain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        // Просто знову повертається у режим шукання
        stateMachine.ChangeState(enemy.PatrolState);
    }
}