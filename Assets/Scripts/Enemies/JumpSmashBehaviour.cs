using System.Collections;
using UnityEngine;

// Boss: leaps toward the player and slams an area on landing, then recovers
public class JumpSmashBehaviour : IEnemyBehaviour
{
    public string Name => "Jump Smash";

    const float JumpTime = 0.6f;
    const float SmashRadius = 3f;

    public IEnumerator Attack(EnemyAI enemy)
    {
        enemy.StopMoving();
        enemy.FacePlayer();
        enemy.PlayAnim("Taunt");
        yield return new WaitForSeconds(0.4f);

        // leap in an arc toward the player's position
        Vector3 start = enemy.transform.position;
        Vector3 target = enemy.PlayerPosition(3f);
        float t = 0f;
        while (t < JumpTime)
        {
            float f = t / JumpTime;
            Vector3 flat = Vector3.Lerp(start, target, f);
            enemy.SetPosition(flat + Vector3.up * Mathf.Sin(f * Mathf.PI) * 1.6f);
            t += Time.deltaTime;
            yield return null;
        }
        enemy.SetPosition(new Vector3(target.x, start.y, target.z));

        // slam
        enemy.PlayAnim("Attack");
        enemy.DamageInRadius(SmashRadius);
        yield return new WaitForSeconds(1.2f);
    }
}
