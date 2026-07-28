using UnityEngine;

// Anything the player can pick up in the world
public interface ICollectable
{
    void Collect(GameObject collector);
}
