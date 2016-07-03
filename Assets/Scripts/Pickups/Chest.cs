using UnityEngine;
using System.Collections.Generic;

class Chest : MonoBehaviour
{
    static float initialDropVelocity = 7;
    public int rank = 1;
    List<GameObject> contents = new List<GameObject>();
    SpriteRenderer sr;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Chest/closed" + rank);
        if(rank == 1)
        {
            Add(Item.Create().gameObject);
            Add(OrbPickup.Create());
            for (int i = 0; i < 3; i++)
            {
                Add(Pixel.Create());
            }
        }
    }

    void Add(GameObject g)
    {
        g.SetActive(false);
        contents.Add(g);
    }

    bool isOpen = false;
    public void Open()
    {
        if (isOpen)
            return;
        isOpen = true;
        sr.sprite = Resources.Load<Sprite>("Chest/open" + rank);
        int protate = Player.instance.transform.position.x > transform.position.x ? 1 : -1;
        foreach (var a in contents)
        {
            a.SetActive(true);
            a.GetComponent<Rigidbody2D>().velocity = new Vector2(0, initialDropVelocity)
                .Rotate(Random.Range(-Mathf.PI / 4, Mathf.PI / 4))
                .Rotate(Mathf.PI / 4 * protate);
            a.transform.position = transform.position;
        }
    }
}
