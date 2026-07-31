using System.Collections;

// An enemy's attack style (Strategy). Each returns a coroutine sequence.
public interface IEnemyBehaviour
{
    string Name { get; }
    IEnumerator Attack(EnemyAI enemy);
}

public enum EnemyBehaviourType
{
    Simple,     // zombie: step in and swipe
    Ram,        // maw: wind up, charge, recover
    JumpSmash,   // boss: leap and slam an area
    PhaseStrike  //Adding enemey: flashstep 
}
