using System.Collections.Generic;
using UnityEngine;

namespace Quiver.Slime
{
  public abstract class PoolManager<T> : MonoBehaviour where T : MonoBehaviour, IPoolObject<T>
  {
    public T prefab;
    protected Queue<T> pool;
    private Transform cacheTransform;

    protected virtual void Awake()
    {
      pool = new Queue<T>();
    }

    public T GetOrCreate()
    {
      var poolObject = (pool.Count > 0) ? pool.Dequeue() : Instantiate(prefab, GetTransform());
      poolObject.PoolManager = this;
      poolObject.ResetObject();
      return poolObject;
    }

    public void Add(T t)
    {
      t.OnAddPool();
      pool.Enqueue(t);
    }

    public Transform GetTransform()
    {
      if (cacheTransform == null)
        cacheTransform = transform;
      return cacheTransform;
    }
  }

  public interface IPoolObject<T> where T : MonoBehaviour, IPoolObject<T>
  {
    PoolManager<T> PoolManager { get; set; }
    void BackToPool();
    void ResetObject();
    void OnAddPool();
  }
}
