using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyBrain enemy, EnemyStateMachine stateMachine)
        : base(enemy, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("ATTACK STATE!");

        // Player вже міг бути знищений
        if (enemy.player == null)
        {
            stateMachine.ChangeState(enemy.PatrolState);
            return;
        }

        PlayerHealth playerHealth = enemy.player.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            Debug.Log("PLAYER HEALTH FOUND!");
            playerHealth.TakeDamage();
        }
        else
        {
            Debug.Log("PLAYER HEALTH NOT FOUND!");
        }

        // Після атаки повертаємося в Chase.
        // Якщо Player помер, ChaseState просто дочекається respawn.
        stateMachine.ChangeState(enemy.ChaseState);
    }
}



/*using UnityEngine;

public class AttackState : EnemyState
{
    public AttackState(EnemyBrain enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        Debug.Log("ATTACK STATE!");

        PlayerHealth player = enemy.player.GetComponent<PlayerHealth>();

        if (player != null)
        {
            Debug.Log("PLAYER HEALTH FOUND!");
            player.TakeDamage();
        }
        else
        {
            Debug.Log("PLAYER HEALTH NOT FOUND!");
        }
    }
}
*/