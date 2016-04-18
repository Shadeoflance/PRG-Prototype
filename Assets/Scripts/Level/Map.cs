using System;
using UnityEngine;

class Map
{
    SubRoom[,] rooms;
    public int size;

    public Map(int size)
    {
        rooms = new SubRoom[size, size];
        this.size = size;
    }

    public Pair<int, int> GetIndex(SubRoom room)
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (rooms[i, j] == room)
                    return new Pair<int, int>(i, j);
            }
        }
        return null;
    }

    public SubRoom GetRelativeTo(SubRoom room, Vector2 dir)
    {
        var index = GetIndex(room);
        return this[index.first + (int)dir.x, index.second + (int)dir.y];
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
            rooms[x, y] = value;
            value.transform.position = new Vector3(Level.roomSize.x * x, Level.roomSize.y * y, 0);
            value.transform.SetParent(Level.instance.transform);
            value.WrapInRoom();
        }
    }

    public void FixDoors()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                SubRoom room = this[x,y];
                if(room == null)
                    continue;
                if (this[x + 1, y] == null || (room.room.subRooms.Contains(this[x + 1, y])))
                    GameObject.Destroy(room.rightD.gameObject);
                if (this[x - 1, y] == null || (room.room.subRooms.Contains(this[x - 1, y])))
                    GameObject.Destroy(room.leftD.gameObject);
                if (this[x, y + 1] == null || (room.room.subRooms.Contains(this[x, y + 1])))
                    GameObject.Destroy(room.topD.gameObject);
                if (this[x, y - 1] == null || (room.room.subRooms.Contains(this[x, y - 1])))
                    GameObject.Destroy(room.botD.gameObject);
            }
        }
    }

    bool CheckBounds(int x, int y)
    {
        return x >= 0 && x < size && y >= 0 && y < size;
    }
}