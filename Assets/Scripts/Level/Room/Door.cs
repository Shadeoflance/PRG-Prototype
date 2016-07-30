using UnityEngine;

public class Door : MonoBehaviour
{
    public Sprite openSprite, closedSprite;
    [System.NonSerialized]
    public SpritePainter painter;
    Color disabledColor = new Color(0.2f, 0.2f, 0.2f);
    bool open = false, disabled;
    SubRoom subRoom;

    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = closedSprite;
        painter = GetComponent<SpritePainter>();
    }

    void Start()
    {
        subRoom = transform.parent.parent.GetComponent<SubRoom>();
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

    void OnCollisionStay2D(Collision2D collision)
    {
        DoorTouch(collision);
    }

    void DoorTouch(Collision2D collision)
    {
        if (disabled)
            return;
        if (open && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            subRoom.DoorTouch(this);
    }
}