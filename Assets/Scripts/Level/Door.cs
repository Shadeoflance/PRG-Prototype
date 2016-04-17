using UnityEngine;

class Door : MonoBehaviour
{
    public Sprite openSprite, closedSprite;
    bool open = false;

    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = closedSprite;
    }

    public void Open()
    {
        open = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
    }

    public void Close()
    {
        open = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(open && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            transform.parent.GetComponent<SubRoom>().DoorTouch(this);
    }
}