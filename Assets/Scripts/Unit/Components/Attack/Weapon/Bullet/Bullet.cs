using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public Vector2 needVel;
    public float speed;
    public float? life = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = needVel * speed;
        if (life != null)
        {
            Debug.Log("Life = " + life);
            life -= Time.deltaTime;
            if (life < 0)
                Destroy(gameObject);
        }
    }
}