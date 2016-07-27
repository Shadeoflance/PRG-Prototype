using System;
using System.Collections.Generic;
using UnityEngine;

class Map
{
    public static Vector2 roomUIDistance = new Vector2(40, 30);
    public static Vector2[] dirs = new Vector2[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    SubRoom[,] rooms;
    public int size;
    public Spawn spawn;
    public BossRoom boss;
    public List<SubRoom> regulars = new List<SubRoom>();

    public Map(int size)
    {
        rooms = new SubRoom[size, size];
        this.size = size;
    }

    public SubRoom GetRelativeTo(SubRoom room, Vector2 dir)
    {
        return this[room.x + (int)dir.x, room.y + (int)dir.y];
    }

    public SubRoom this[int x, int y]
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
            value.WrapInRoom();
            value.Disable();
            if(value is Spawn)
            {
                spawn = (Spawn)value;
            }
            if(value is BossRoom)
            {
                boss = (BossRoom)value;
            }
            if(value.GetType() == typeof(SubRoom))
            {
                regulars.Add(value);
            }
        }
    }

    public void PostInitialize()
    {
        List<Room> processedRooms = new List<Room>();
        foreach (var a in rooms)
        {
            if (a != null)
            {
                if (!processedRooms.Contains(a.room))
                {
                    a.room.InitUI();
                    processedRooms.Add(a.room);
                }
            }
        }
    }

    public void UpdateUI()
    {
        List<Room> processedRooms = new List<Room>();
        foreach (var a in rooms)
        {
            if (a != null && !processedRooms.Contains(a.room))
            {
                a.room.roomUI.Update();
                processedRooms.Add(a.room);
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
                a.room.roomUI.Reveal();
        }
    }
}