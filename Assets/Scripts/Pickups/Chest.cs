using UnityEngine;
using System.Collections.Generic;

class Chest : MonoBehaviour
{
    static float initialDropVelocity = 7;
    public int rank = 1;
    public SpriteRenderer top;
    public int id;
    List<GameObject> contents = new List<GameObject>();

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
        if (rank == 1)
        {
            Add(Item.Create(id).gameObject);
            Add(OrbPickup.Create());
            for (int i = 0; i < 3; i++)
            {
                Add(Pixel.Create());
            }
        }

        isOpen = true;
        top.color = new Color(0f, 0f, 0f, 0f);
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
