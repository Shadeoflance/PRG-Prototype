using UnityEngine;

class Gold : MonoBehaviour
{
    int amount = 1;

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            col.gameObject.GetComponent<Player>().AddGold(amount);
            Destroy(gameObject);
        }
    }

    static float initialDropVelocity = 10;
    static GameObject prefab;
    public static void Drop(int amountOfDrops, Vector2 position)
    {
        if(prefab == null)
            prefab = Resources.Load<GameObject>("Pickups/Gold");
        for (int i = 0; i < amountOfDrops; i++)
        {
            GameObject instance = Instantiate(prefab);
            instance.GetComponent<Rigidbody2D>().velocity = new Vector2(0, initialDropVelocity).Rotate(Random.Range(-Mathf.PI / 4, Mathf.PI / 4));
            instance.transform.position = position;
        }
    }
}