using UnityEngine;
using UnityEngine.Pool;

public class RequiemProjectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private int damage;
    private Vector3 startPosition;
    private float maxRange = 8f;
    private bool isReleased;

    private ObjectPool<RequiemProjectile> pool;

    public void Init(Vector3 dir, float spd, int dmg, ObjectPool<RequiemProjectile> pool)
    {
        isReleased = false;
        direction = dir;
        speed = spd;
        damage = dmg;
        this.pool = pool;
        startPosition = transform.position;
        AudioManager.Instance.Play("Requiem");
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) >= maxRange)
            ReturnToPool();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Enemy>().TakeDamage(damage);
        // pierce through enemies, so we don't return to pool here
    }

    private void ReturnToPool()
    {
        if (isReleased) return;
        isReleased = true;
        pool.Release(this);
    }
}
