using UnityEngine;

// First spots the player: faces them and taunts (flex) before giving chase
public class EnemyAlertState : IEnemyState
{
    private float timer;

    public void Enter(EnemyAI enemy)
    {
        enemy.StopMoving();
        enemy.FacePlayer();
        enemy.TriggerTaunt();
        timer = 1.3f;
    }

    public void Tick(EnemyAI enemy)
    {
        enemy.FacePlayer();
        timer -= Time.deltaTime;
        if (timer <= 0f)
            enemy.SetState(new EnemyChaseState());
    }
}
