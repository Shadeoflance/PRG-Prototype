using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 needVel;
    public float speed;
    public float? life = null;
    public Unit player;
    public int dmg;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = needVel * speed;
        if (life != null)
        {
            life -= Time.deltaTime;
            if (life < 0)
                Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            ActionParams ap = new ActionParams();
            ap["other"] = enemy.gameObject;
            ap["bullet"] = this;
            ap["dmg"] = dmg;
            player.eventManager.InvokeInterceptors("bulletEnemyHit", ap);
            if (!ap.forbid)
            {
                player.eventManager.InvokeHandlers("bulletDestroy", null);
                Destroy(gameObject);
            }
            enemy.currentState.DealDamage((int)ap.parameters["dmg"], gameObject);
            ap = new ActionParams();
            ap["enemy"] = enemy;
            player.eventManager.InvokeHandlers("bulletEnemyHit", ap);
        }
        if (collision.tag.Equals("Level"))
        {
            ActionParams ap = new ActionParams();
            ap["other"] = collision.gameObject;
            ap["bullet"] = this;
            player.eventManager.InvokeInterceptors("bulletLevelHit", ap);
            if (!ap.forbid)
            {
                ap = new ActionParams();
                ap["bullet"] = this;
                player.eventManager.InvokeHandlers("bulletDestroy", ap);
                Destroy(gameObject);
            }
        }
    }
}