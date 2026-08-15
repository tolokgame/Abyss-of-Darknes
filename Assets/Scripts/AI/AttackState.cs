using UnityEngine;

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
