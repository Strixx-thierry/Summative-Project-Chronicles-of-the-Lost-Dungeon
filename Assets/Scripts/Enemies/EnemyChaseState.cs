// Runs toward the player; attacks when close, gives up if the player escapes
public class EnemyChaseState : IEnemyState
{
    public void Enter(EnemyAI enemy) { }

    public void Tick(EnemyAI enemy)
    {
        float d = enemy.DistanceToPlayer();
        if (d > enemy.DetectRange * 1.3f) { enemy.SetState(new EnemyIdleState()); return; }
        if (d <= enemy.AttackRange) { enemy.SetState(new EnemyAttackState()); return; }
        enemy.MoveToward(enemy.Player.position);
    }
}
