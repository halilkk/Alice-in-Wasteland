using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class WrathProjectile : MonoBehaviour
{

    private ObjectPool<WrathProjectile> pool;

    public void Init(ObjectPool<WrathProjectile> pool)
    {
        this.pool = pool;
        StartCoroutine(Return());
    }
    IEnumerator Return()    
    {
        yield return new WaitForSeconds(0.2f);
        ReturntoPool();
    }

    private void ReturntoPool()
    {
        pool.Release(this);
    }
}