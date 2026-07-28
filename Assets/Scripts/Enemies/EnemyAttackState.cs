using UnityEngine;

// Faces the player and runs the enemy's behaviour attack on a cooldown; chases if the player escapes
public class EnemyAttackState : IEnemyState
{
    private float timer;

    public void Enter(EnemyAI enemy)
    {
        enemy.StopMoving();
        timer = 0.3f;   // small wind-up before the first attack
    }

    public void Tick(EnemyAI enemy)
    {
        if (enemy.Busy) return;   // a behaviour attack (ram/smash) is playing out

        if (enemy.DistanceToPlayer() > enemy.AttackRange + 0.6f)
        {
            enemy.SetState(new EnemyChaseState());
            return;
        }

        enemy.StopMoving();
        enemy.FacePlayer();

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            enemy.StartAttack();
            timer = enemy.AttackCooldown;
        }
    }
}
