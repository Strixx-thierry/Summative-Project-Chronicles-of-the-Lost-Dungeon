// Stands still until the player comes within detection range
public class EnemyIdleState : IEnemyState
{
    public void Enter(EnemyAI enemy) => enemy.StopMoving();

    public void Tick(EnemyAI enemy)
    {
        if (enemy.Player == null) return;
        if (enemy.DistanceToPlayer() <= enemy.DetectRange)
            enemy.SetState(new EnemyChaseState());
    }
}
