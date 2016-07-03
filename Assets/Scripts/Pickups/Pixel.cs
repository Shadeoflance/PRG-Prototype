using UnityEngine;
using Math = System.Math;

class Pixel : MonoBehaviour
{
    int amount = 1;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.IsPlayer())
        {
            col.gameObject.GetComponent<Player>().AddPixel(amount);
            Destroy(transform.parent.gameObject);
        }
    }

    static float initialDropVelocity = 10;
    static Prefab prefab = new Prefab("Pickups/Pixel");
    public static void Drop(int amountOfDrops, Vector2 position)
    {
        for (int i = 0; i < amountOfDrops; i++)
        {
            GameObject instance = prefab.Instantiate();
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, initialDropVelocity).Rotate(Random.Range(-Mathf.PI / 4, Mathf.PI / 4));
            instance.transform.position = position;
        }
    }
    public static GameObject Create()
    {
        return prefab.Instantiate().gameObject;
    }
}