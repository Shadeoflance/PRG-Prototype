using System.Collections.Generic;
using UnityEngine;

class Room
{
    public List<SubRoom> subRooms = new List<SubRoom>();

    public Room(SubRoom subRoom)
    {
        subRooms.Add(subRoom);
    }

    public void Unite(Room r)
    {
        foreach (var a in r.subRooms)
        {
            a.room = this;
            subRooms.Add(a);
        }
        foreach (var a in subRooms)
        {
            if (subRooms.Contains(Level.instance.map.GetRelativeTo(a, Vector2.left)))
                GameObject.Destroy(a.leftW);
            if (subRooms.Contains(Level.instance.map.GetRelativeTo(a, Vector2.right)))
                GameObject.Destroy(a.rightW);
            if (subRooms.Contains(Level.instance.map.GetRelativeTo(a, Vector2.down)))
                GameObject.Destroy(a.botW);
            if (subRooms.Contains(Level.instance.map.GetRelativeTo(a, Vector2.up)))
                GameObject.Destroy(a.topW);
        }
    }

    public void EnemyDied(Enemy enemy)
    {
        foreach (var a in subRooms)
            a.EnemyDied(enemy);
        foreach (var a in subRooms)
            if (a.enemiesAlive.Count > 0)
                return;
        EnableDoors();
    }

    public void Enable()
    {
        foreach (var a in subRooms)
            a.Enable();
    }

    public void Disable()
    {
        foreach (var a in subRooms)
            a.Disable();
    }

    public void EnableDoors()
    {
        foreach (var a in subRooms)
            a.EnableDoors();
    }

    public void DisableDoors()
    {
        foreach (var a in subRooms)
            a.DisableDoors();
    }
}