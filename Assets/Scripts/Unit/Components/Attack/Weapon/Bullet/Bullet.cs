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
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            ActionParams ap = new ActionParams();
            ap.me = player.gameObject;
            ap.other = enemy.gameObject;
            ap.bullet = this;
            ap.intParam = dmg;
            player.eventManager.InvokeInterceptors("enemyHit", ap);
            if (!ap.forbid)
            {
                player.eventManager.InvokeHandlers("bulletDestroy");
                Destroy(gameObject);
            }
            enemy.currentState.Damage(ap.intParam);
            player.eventManager.InvokeHandlers("enemyHit");
        }
        if (collision.tag.Equals("Level"))
        {
            ActionParams ap = new ActionParams();
            ap.me = player.gameObject;
            ap.other = collision.gameObject;
            ap.bullet = this;
            player.eventManager.InvokeInterceptors("levelHit", ap);
            if (!ap.forbid)
            {
                player.eventManager.InvokeHandlers("bulletDestroy");
                Destroy(gameObject);
            }
        }
    }
}