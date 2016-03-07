using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public int id;
    public Action<Player> getAction;

    void Awake()
    {
        if (id == 0)
            ItemPool.AssignItem(this);
        else ItemPool.AssignItemWithId(this, id);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            getAction(collision.gameObject.GetComponent<Player>());
            Destroy(gameObject);
        }
    }
}