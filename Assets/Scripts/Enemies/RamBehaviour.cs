using System.Collections;
using UnityEngine;

// Maw: winds up, then charges fast at the player, then recovers
public class RamBehaviour : IEnemyBehaviour
{
    public string Name => "Ram";

    const float ChargeSpeed = 11f;
    const float ChargeTime = 0.55f;

    public IEnumerator Attack(EnemyAI enemy)
    {
        // wind up
        enemy.StopMoving();
        enemy.FacePlayer();
        enemy.PlayAnim("Taunt");
        yield return new WaitForSeconds(0.5f);

        // lock direction and charge
        Vector3 dir = enemy.DirectionToPlayer();
        enemy.PlayAnim("Attack");
        float t = 0f;
        bool hit = false;
        while (t < ChargeTime)
        {
            enemy.ChargeToward(dir, ChargeSpeed);
            if (!hit && enemy.DistanceToPlayer() <= enemy.AttackRange + 0.7f)
            {
                enemy.TryDamagePlayer();
                hit = true;
            }
            t += Time.deltaTime;
            yield return null;
        }

        // recover (staggered, vulnerable)
        enemy.StopMoving();
        yield return new WaitForSeconds(1.1f);
    }
}
