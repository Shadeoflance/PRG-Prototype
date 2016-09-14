using UnityEngine;
using System;
using System.Collections;

public class Item : MonoBehaviour
{
    public Action getAction;
    public int id;
    public bool canPickup = false;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(Lift());
        rb.velocity = Vector2.zero;
        ItemPool.AssignItem(this, id);
    }

    IEnumerator Lift()
    {
        float t = 2;
        while(t > 0)
        {
            t -= Time.deltaTime;
            transform.position += new Vector3(0, Time.deltaTime / 2 * t);
            yield return null;
        }
        canPickup = true;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.IsPlayer() && canPickup && Math.Abs(rb.velocity.y) < 0.01f)
        {
            canPickup = false;
            PickUp();
        }
    }

    public void PickUp()
    {
        Debug.Log("Item picked up: " + id);
        getAction();
        Destroy(gameObject);
    }

    static Prefab prefab = new Prefab("Item");

    public static Item Create(int id = 0)
    {
        Item i = prefab.Instantiate().GetComponent<Item>();
        i.id = id;
        return i;
    }
}