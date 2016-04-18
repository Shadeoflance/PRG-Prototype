using UnityEngine;

class Door : MonoBehaviour
{
    public Sprite openSprite, closedSprite;
    bool open = false;
    SubRoom subRoom;

    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        subRoom = transform.parent.GetComponent<SubRoom>();
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

    void OnCollisionStay2D(Collision2D collision)
    {
        if(open && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            subRoom.DoorTouch(this);
    }
}