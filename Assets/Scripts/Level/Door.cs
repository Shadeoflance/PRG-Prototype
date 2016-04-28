using UnityEngine;

class Door : MonoBehaviour
{
    public Sprite openSprite, closedSprite;
    public SpritePainter painter;
    Color disabledColor = new Color(0.2f, 0.2f, 0.2f);
    bool open = false, disabled;
    SubRoom subRoom;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        subRoom = transform.parent.parent.GetComponent<SubRoom>();
        painter = GetComponent<SpritePainter>();
    }

    public void Open()
    {
        if (disabled)
            return;
        open = true;
        GetComponent<SpriteRenderer>().sprite = openSprite;
    }

    public void Close()
    {
        open = false;
        GetComponent<SpriteRenderer>().sprite = closedSprite;
    }

    public void Disable()
    {
        disabled = true;
        painter.Paint(disabledColor);
    }

    public void Enable()
    {
        disabled = false;
        painter.Clear();
        painter.Paint(disabledColor, 0.5f, true);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        DoorTouch(collision);
    }

    //void OnCollisionStay2D(Collision2D collision)
    //{
    //    DoorTouch(collision);
    //}

    void DoorTouch(Collision2D collision)
    {
        if (disabled)
            return;
        if (open && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            subRoom.DoorTouch(this);
    }
}