using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public Action getAction;
    public int id;
    public bool canPickup = true;
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        ItemPool.AssignItem(this);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.IsPlayer() && canPickup && Math.Abs(rb.velocity.y) < 0.01f)
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        getAction();
        Destroy(gameObject);
    }

    static Prefab prefab = new Prefab("Item");

    public static Item Create()
    {
        return prefab.Instantiate().GetComponent<Item>();
    }
}