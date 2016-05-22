using UnityEngine;
using Math = System.Math;

class Gold : MonoBehaviour
{
    int amount = 1;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.IsPlayer() && Math.Abs(rb.velocity.y) < 0.01f)
        {
            col.gameObject.GetComponent<Player>().AddGold(amount);
            Destroy(gameObject);
        }
    }

    static float initialDropVelocity = 10;
    static Prefab prefab = new Prefab("Pickups/Gold");
    public static void Drop(int amountOfDrops, Vector2 position)
    {
        for (int i = 0; i < amountOfDrops; i++)
        {
            GameObject instance = prefab.Instantiate();
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, initialDropVelocity).Rotate(Random.Range(-Mathf.PI / 4, Mathf.PI / 4));
            instance.transform.position = position;
        }
    }
    public static Gold Create()
    {
        return prefab.Instantiate().GetComponent<Gold>();
    }
}