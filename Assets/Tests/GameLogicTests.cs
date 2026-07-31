using NUnit.Framework;
using UnityEngine;

// Eight unit tests for the game's pure C# logic (no scenes, no play mode).
// Covers: damage calculation, maths/formatting, save/load, level unlocking and object pooling.
public class GameLogicTests
{
    // ---------- Damage calculation (algorithm + maths) ----------

    [Test]
    public void Damage_NoDefenceNoCrit_ReturnsBase()
    {
        Assert.AreEqual(40, DamageCalculator.Compute(40, 0, 0f));
    }

    [Test]
    public void Damage_WithDefence_IsReduced()
    {
        // 40 * (100 / (100 + 100)) = 20
        Assert.AreEqual(20, DamageCalculator.Compute(40, 100, 0f));
    }

    [Test]
    public void Damage_NeverBelowOne()
    {
        Assert.GreaterOrEqual(DamageCalculator.Compute(1, 100000, 0f), 1);
    }

    // ---------- Maths / formatting ----------

    [Test]
    public void RunStats_Format_ProducesMinutesSeconds()
    {
        Assert.AreEqual("01:05", RunStats.Format(65f));
        Assert.AreEqual("02:00", RunStats.Format(120f));
    }

    // ---------- Save / load (JSON serialization) ----------

    [Test]
    public void SaveData_JsonRoundTrip_PreservesData()
    {
        var data = new SaveData { playerName = "Hero", selectedClass = 2, highestUnlocked = 3 };
        data.completed.Add(1);
        data.completed.Add(2);

        var json = JsonUtility.ToJson(data);
        var loaded = JsonUtility.FromJson<SaveData>(json);

        Assert.AreEqual("Hero", loaded.playerName);
        Assert.AreEqual(3, loaded.highestUnlocked);
        Assert.Contains(2, loaded.completed);
    }

    // ---------- Level unlocking (progression) ----------

    [Test]
    public void Progression_Complete_UnlocksNextLevel()
    {
        var data = new SaveData();
        Progression.Complete(data, 1, 10, 5f);
        Assert.AreEqual(2, data.highestUnlocked);
        Assert.IsTrue(Progression.IsUnlocked(data, 2));
        Assert.IsFalse(Progression.IsUnlocked(data, 3));
    }

    [Test]
    public void Progression_Complete_KeepsFasterTime()
    {
        var data = new SaveData();
        Progression.Complete(data, 1, 0, 10f);
        Progression.Complete(data, 1, 0, 7f);   // faster, should replace
        Assert.AreEqual(7f, data.bestTimes[0], 0.001f);
        Progression.Complete(data, 1, 0, 9f);   // slower, ignored
        Assert.AreEqual(7f, data.bestTimes[0], 0.001f);
    }

    // ---------- Object pooling behaviour ----------

    [Test]
    public void ObjectPool_ReturnedObject_IsRecycled()
    {
        var prefabGo = new GameObject("PoolItem");
        var prefab = prefabGo.AddComponent<BoxCollider>();
        var pool = new ObjectPool<BoxCollider>(prefab, 1);

        var first = pool.Get();
        Assert.IsTrue(first.gameObject.activeSelf, "Get should activate the object");

        pool.Return(first);
        Assert.IsFalse(first.gameObject.activeSelf, "Return should deactivate the object");

        var second = pool.Get();
        Assert.AreSame(first, second, "The pool should reuse the returned instance");

        Object.DestroyImmediate(prefabGo);
    }
}
