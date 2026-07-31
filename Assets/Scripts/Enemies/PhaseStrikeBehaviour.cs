using System.Collections;
using UnityEngine;

public class PhaseStrikeBehaviour : IEnemyBehaviour
{
    public string Name => "Phase Strike";

    const float VanishTime = 0.35f;
    const float BehindOffset = 1.4f;    // how far behind the player it reappears
    const float TellTime = 0.15f;       // a beat to let the player react
    const float StrikeRadius = 2f;
    const float RetreatDistance = 6f;
    const float RecoverTime = 0.9f;

    public IEnumerator Attack(EnemyAI enemy)
    {
        // vanish
        enemy.StopMoving();
        enemy.FacePlayer();
        enemy.PlayAnim("Taunt");
        yield return new WaitForSeconds(VanishTime);

        if (enemy.Player == null) yield break;

        Vector3 behind = enemy.Player.position - enemy.Player.forward * BehindOffset;
        enemy.SetPosition(new Vector3(behind.x, enemy.transform.position.y, behind.z));

        // turn to face while the tell plays out
        float t = 0f;
        while (t < TellTime)
        {
            enemy.FacePlayer();
            t += Time.deltaTime;
            yield return null;
        }

        // strike
        enemy.PlayAnim("Attack");
        yield return new WaitForSeconds(0.2f);
        enemy.DamageInRadius(StrikeRadius);
        yield return new WaitForSeconds(0.25f);

        // blink clear; the state machine walks it back in for the next pass
        Vector3 exit = enemy.transform.position - enemy.DirectionToPlayer() * RetreatDistance;
        enemy.SetPosition(new Vector3(exit.x, enemy.transform.position.y, exit.z));
        yield return new WaitForSeconds(RecoverTime);
    }
}
