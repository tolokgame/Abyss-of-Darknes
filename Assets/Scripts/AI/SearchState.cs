using UnityEngine;

public class SearchState : EnemyState
{
    private float searchTimer;

    public SearchState(EnemyBrain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        searchTimer = enemy.searchDuration;
        enemy.agent.SetDestination(enemy.lastKnownPlayerPosition);
    }

    public override void Update()
    {
        // якщо п≥д час пошуку знову пом≥тили гравц€ -> знову б≥жимо за ним!
        if (enemy.CanSeePlayer())
        {
            stateMachine.ChangeState(enemy.ChaseState);
            return;
        }

        //  оли приб≥гли на останнЇ м≥сце зустр≥ч≥
        if (!enemy.agent.pathPending && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0)
            {
                // „ас вийшов, гравц€ немаЇ -> йдемо в стан поверненн€
                stateMachine.ChangeState(enemy.ReturnState);
            }
        }
    }
}