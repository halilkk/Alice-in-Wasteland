using UnityEngine;
using UnityEngine.Pool;

public class HexProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damage;
    private Vector3 startPosition;
    private float maxRange = 15f;
    private bool isReleased;

    private ObjectPool<HexProjectile> pool;

    public void Init(Vector3 dir, float spd, float dmg, ObjectPool<HexProjectile> pool)
    {
        isReleased = false;
        direction = dir;
        speed = spd;
        damage = dmg;
        this.pool = pool;
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxRange)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(damage);
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        if (isReleased) return; 
        isReleased = true;
        pool.Release(this); 
    }
}
