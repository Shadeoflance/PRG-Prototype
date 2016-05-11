using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public Action getAction;
    public int id;
    public bool canPickup = true;

    void Awake()
    {
        ItemPool.AssignItem(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player") && canPickup)
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        getAction();
        Destroy(gameObject);
    }

    static GameObject prefab = Resources.Load<GameObject>("Item");

    public static Item Create()
    {
        return Instantiate(prefab).GetComponent<Item>();
    }
}