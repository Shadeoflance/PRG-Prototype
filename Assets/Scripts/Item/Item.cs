using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    public Action getAction;

    void Awake()
    {
        ItemPool.AssignItem(this);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            getAction();
            Destroy(gameObject);
        }
    }
}