using System.Collections.Generic;
using UnityEngine;

public interface IPoolable
{
    public void InitPoolObject();
    public void ResetPoolObject();
}

public abstract class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour, IPoolable
{
    [SerializeField]
    private T prefab;
    protected abstract int InitialSize { get; }

    private readonly Queue<T> pool = new Queue<T>();

    private void Awake()
    {
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
        obj.InitPoolObject();
        return obj;
    }

    public virtual void Return(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.ResetPoolObject();
        pool.Enqueue(obj);
    }
}
