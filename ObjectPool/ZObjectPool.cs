using System;
using System.Collections.Generic;

public class ZObjectPool<T> where T : new()
{
    public int capacity;

    protected delegate T CreationHandler();
    protected CreationHandler OnCreate;

    protected Queue<T> pool = new Queue<T>();

    public int GetPoolSize() => pool.Count;
    public ZObjectPool() => OnCreate = () => new T();

    public ZObjectPool(Func<T> createFunction) => OnCreate = createFunction.Invoke;

    public virtual T Acquire()
    {
        T poolObject;

        if (pool.Count > 0)
        {
            poolObject = pool.Dequeue();
        }
        else
        {
            poolObject = OnCreate.Invoke();
        }
        return poolObject;
    }
    public virtual void Release(T returnObject)
    {
        if (GetPoolSize() > capacity)
        {
            returnObject = default;
            return;
        }
        pool.Enqueue(returnObject);
    }
}
