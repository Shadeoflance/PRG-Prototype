using UnityEngine;
using UnityEngine.UI;

class SubRoomUI
{
    Image image;
    SubRoom subRoom;
    bool revealed = false;

    public SubRoomUI(Image image, SubRoom subRoom)
    {
        this.image = image;
        this.subRoom = subRoom;
    }

    public void Update()
    {
        Level level = Level.instance;
        if (level.current == subRoom.room)
        {
            image.color = Color.white;
            revealed = true;
            return;
        }
        if (!revealed)
        {
            if (level.current.subRooms.Contains(level.map.GetRelativeTo(subRoom, Vector2.up)) ||
                level.current.subRooms.Contains(level.map.GetRelativeTo(subRoom, Vector2.right)) ||
                level.current.subRooms.Contains(level.map.GetRelativeTo(subRoom, Vector2.down)) ||
                level.current.subRooms.Contains(level.map.GetRelativeTo(subRoom, Vector2.left)))
            {
                revealed = true;
                image.color = new Color(0.3f, 0.3f, 0.3f);
            }
            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
        else
        {
            image.color = new Color(0.3f, 0.3f, 0.3f);
        }

    }
}