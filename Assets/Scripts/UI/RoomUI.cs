using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class RoomUI
{
    public List<Image> images = new List<Image>();
    Room room;
    bool revealed = false;
    bool visited = false;

    public RoomUI(Room room)
    {
        this.room = room;
        foreach(var a in room.subRooms)
        {
            Pair<int, int> ind = Level.instance.map.GetIndex(a);
            images.Add(CreateImage(ind.first, ind.second));
        }
        CreateConnectors(room.subRooms[0], room.subRooms, new List<SubRoom>());
    }

    Image CreateImage(float x, float y)
    {
        Image image = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("SubRoomUI")).GetComponent<Image>();
        RectTransform map = GameObject.Find("Map").GetComponent<RectTransform>();
        image.rectTransform.SetParent(map);
        image.rectTransform.position = map.position.ToV2() + Vector2.Scale(Map.roomUIDistance, new Vector2(x, -Level.instance.map.size + 1 + y));
        return image;
    }

    void CreateConnectors(SubRoom current, List<SubRoom> rooms, List<SubRoom> processed)
    {
        foreach (var d in Map.dirs)
        {
            SubRoom next = Level.instance.map.GetRelativeTo(current, d);
            if (next != null && rooms.Contains(next) && !processed.Contains(next))
            {
                Pair<int, int> ind = Level.instance.map.GetIndex(current);
                Vector2 t = new Vector2(ind.first, ind.second) + d * 0.5f;
                Image newInstance = CreateImage(t.x, t.y);
                images.Add(newInstance);
                processed.Add(current);
                if (!processed.Contains(next))
                    CreateConnectors(next, rooms, processed);
            }
            else continue;
        }
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
            foreach (var a in room.subRooms)
            {
                if (level.current.subRooms.Contains(level.map.GetRelativeTo(a, Vector2.up)) ||
                    level.current.subRooms.Contains(level.map.GetRelativeTo(a, Vector2.right)) ||
                    level.current.subRooms.Contains(level.map.GetRelativeTo(a, Vector2.down)) ||
                    level.current.subRooms.Contains(level.map.GetRelativeTo(a, Vector2.left)))
                {
                    revealed = true;
                    SetColor(new Color(0.3f, 0.3f, 0.3f));
                    break;
                }
                else
                {
                    SetColor(new Color(1, 1, 1, 0));
                }
            }
        }
        else
        {
            if (visited)
                SetColor(new Color(0.65f, 0.65f, 0.65f));
            else SetColor(new Color(0.3f, 0.3f, 0.3f));
        }

    }

    void SetColor(Color color)
    {
        foreach (var a in images)
            a.color = color;
    }
}