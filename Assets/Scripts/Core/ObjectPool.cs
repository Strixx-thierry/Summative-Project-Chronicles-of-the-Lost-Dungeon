using UnityEngine;
using System.Collections.Generic;

// Generic reusable-object pool (Object Pooling pattern) for frequently spawned things
public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly Transform parent;
    private readonly Queue<T> available = new Queue<T>();

    public ObjectPool(T prefab, int initialSize)
    {
        this.prefab = prefab;
        parent = new GameObject(prefab.name + "Pool").transform;
        for (int i = 0; i < initialSize; i++) available.Enqueue(Create());
    }

    private T Create()
    {
        var obj = Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T Get()
    {
        var obj = available.Count > 0 ? available.Dequeue() : Create();
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        available.Enqueue(obj);
    }
}
