using UnityEngine;

public abstract class EnemyState
{
    protected EnemyBrain enemy;
    protected EnemyStateMachine stateMachine;

    // Конструктор, щоб кожен стан мав доступ до "мізків" ворога та перемикача станів
    public EnemyState(EnemyBrain enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }  // Спрацьовує ОДИН раз при вході в стан
    public virtual void Update() { } // Спрацьовує кожен кадр (як Update в MonoBehaviour)
    public virtual void Exit() { }   // Спрацьовує ОДИН раз при виході з цього стану
}