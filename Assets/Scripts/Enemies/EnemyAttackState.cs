using UnityEngine;

// Faces the player and swings on a cooldown; chases again if the player backs away
public class EnemyAttackState : IEnemyState
{
    private float timer;

    public void Enter(EnemyAI enemy)
    {
        enemy.StopMoving();
        timer = 0.3f;   // small wind-up before the first swing
    }

    public void Tick(EnemyAI enemy)
    {
        if (enemy.DistanceToPlayer() > enemy.AttackRange + 0.5f)
        {
            enemy.SetState(new EnemyChaseState());
            return;
        }

        enemy.StopMoving();
        enemy.FacePlayer();

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            enemy.TriggerAttack();
            enemy.TryDamagePlayer();
            timer = enemy.AttackCooldown;
        }
    }
}
