// One behaviour state for an enemy (State pattern)
public interface IEnemyState
{
    void Enter(EnemyAI enemy);
    void Tick(EnemyAI enemy);
}
