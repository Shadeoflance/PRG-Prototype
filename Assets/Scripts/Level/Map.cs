using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Map
{
    public static Vector2 roomUIDistance = new Vector2(40, 30);
    public static Vector2[] dirs = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    Room[,] rooms;
    public int size;
    public Spawn spawn;
    public BossRoom boss;
    public List<Room> regulars = new List<Room>();

    public Map(int size)
    {
        rooms = new Room[size, size];
        this.size = size;
    }

    public Room GetRelativeTo(Room room, Vector2 dir)
    {
        return this[room.x + (int)dir.x, room.y + (int)dir.y];
    }

    public Room this[int x, int y]
    {
        get 
        {
            if (!CheckBounds(x, y))
                return null;
            return rooms[x, y]; 
        }
        set 
        {
            if (!CheckBounds(x, y))
                throw new Exception("Tried to set room out of map bounds. x = " + x + " y = " + y);
            if (rooms[x, y] != null)
                throw new Exception("Tried to set new room in position of existing one. x = " + x + " y = " + y);
            rooms[x, y] = value;
            value.x = x;
            value.y = y;
            value.transform.position = new Vector3((Level.roomSize.x) * x, (Level.roomSize.y) * y, 0);
            value.transform.SetParent(Level.instance.transform);
            value.Disable();
            if(value is Spawn)
            {
                spawn = (Spawn)value;
            }
            if(value is BossRoom)
            {
                boss = (BossRoom)value;
            }
            if(value.GetType() == typeof(Room))
            {
                regulars.Add(value);
            }
        }
    }

    public void PostInitialize()
    {
        foreach (var a in rooms)
        {
            if (a != null)
            {
                a.InitUI();
            }
        }
        Transform m = GameObject.Find("Map").transform;
        Image rui = regulars[0].roomUI.image;
        int curPixels = (int)Math.Round(rooms.GetLength(0) * rui.mainTexture.width * rui.transform.localScale.x + 
            roomUIDistance.x * (rooms.GetLength(0) - 1));
        int needPixels = Screen.width / 10;
        m.transform.localScale = new Vector3(needPixels * 1f / curPixels, needPixels * 1f / curPixels, 1);
    }

    public void UpdateUI()
    {
        List<Room> processedRooms = new List<Room>();
        foreach (var a in rooms)
        {
            if (a != null)
            {
                a.roomUI.Update();
            }
        }
    }

    public bool CheckBounds(int x, int y)
    {
        return x >= 0 && x < size && y >= 0 && y < size;
    }

    public void Reveal()
    {
        foreach(var a in rooms)
        {
            if (a != null)
                a.roomUI.Reveal();
        }
    }
}