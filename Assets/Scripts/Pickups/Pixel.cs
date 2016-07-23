using UnityEngine;

class Pixel : MonoBehaviour
{
    int amount = 1;
    public Rigidbody2D rb;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(rb.velocity.magnitude < 0.1f && col.gameObject.IsPlayer())
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
            instance.transform.parent = Level.instance.current.subRooms[0].transform;
        }
    }
    public static GameObject Create()
    {
        return prefab.Instantiate().gameObject;
    }
}