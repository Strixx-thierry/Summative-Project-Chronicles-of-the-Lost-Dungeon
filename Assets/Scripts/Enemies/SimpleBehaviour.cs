using System.Collections;
using UnityEngine;

// Zombie: a plain swipe in front
public class SimpleBehaviour : IEnemyBehaviour
{
    public string Name => "Swipe";

    public IEnumerator Attack(EnemyAI enemy)
    {
        enemy.StopMoving();
        enemy.FacePlayer();
        enemy.PlayAnim("Attack");
        yield return new WaitForSeconds(0.3f);
        enemy.TryDamagePlayer();
        yield return new WaitForSeconds(0.5f);
    }
}
