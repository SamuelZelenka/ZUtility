using UnityEngine;

public class ZGameObjectPool : ZObjectPool<GameObject>
{
    private GameObject prefab;
    private Transform parent;

    public override GameObject Acquire()
    {
        GameObject acquired = base.Acquire();
        acquired.SetActive(true);
        return acquired;
    }

    public override void Release(GameObject releaseObject)
    {
        if (GetPoolSize() > capacity)
        {
            Object.Destroy(releaseObject);
            return;
        }
        pool.Enqueue(releaseObject);
        releaseObject.SetActive(false);
    }
    public ZGameObjectPool(GameObject prefab, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;
        OnCreate = () => Object.Instantiate(prefab.gameObject, parent);
    }
}
