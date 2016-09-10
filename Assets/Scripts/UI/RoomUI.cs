using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomUI
{
    public Image image;
    public Color initialColor;
    Room room;
    bool revealed = false;
    bool visited = false;

    public RoomUI(Room room)
    {
        this.room = room;
        initialColor = room.RoomColor;
        image = CreateImage(room.x, room.y);
    }

    Image CreateImage(float x, float y)
    {
        Image image = Object.Instantiate(Resources.Load<GameObject>("SubRoomUI")).GetComponent<Image>();
        RectTransform map = GameObject.Find("Map").GetComponent<RectTransform>();
        image.rectTransform.SetParent(map);
        image.rectTransform.position = map.position.ToV2() + Vector2.Scale(Map.roomUIDistance, new Vector2(x, -Level.instance.map.size + 1 + y));
        return image;
    }

    public void Update()
    {
        Level level = Level.instance;
        if (level.current == room)
        {
            SetColor(Color.white);
            revealed = true;
            visited = true;
            return;
        }
        if (!revealed)
        {
                if (level.current == level.map.GetRelativeTo(room, Vector2.up))
                {
                    revealed = true;
                    SetColor(new Color(0.3f, 0.3f, 0.3f));
                }
                else
                {
                    SetColor(new Color(1, 1, 1, 0));
                }
        }
        else
        {
            if (visited)
                SetColor(new Color(0.65f, 0.65f, 0.65f));
            else SetColor(new Color(0.3f, 0.3f, 0.3f));
        }

    }

    public void Reveal()
    {
        revealed = true;
        Update();
    }

    void SetColor(Color color)
    {
        image.color = color * initialColor;
    }
}