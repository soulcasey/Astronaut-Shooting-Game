using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    public void Init();
    public void Reset();
}

public abstract class ObjectPool<T> : SingletonBase<ObjectPool<T>> where T : MonoBehaviour, IPoolable
{
    [SerializeField]
    private T prefab;
    protected abstract int InitialSize { get; }

    private readonly Queue<T> pool = new Queue<T>();

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < InitialSize; i++)
        {
            AddObjectToPool();
        }
    }

    private void AddObjectToPool()
    {
        T obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    public virtual T Get()
    {
        if (pool.Count == 0) AddObjectToPool();

        T obj = pool.Dequeue();
        obj.gameObject.SetActive(true);
        obj.Init();
        return obj;
    }

    public virtual void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.Reset();
        pool.Enqueue(obj);
    }
}
