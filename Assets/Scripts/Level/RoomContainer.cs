using UnityEngine;
using System.Collections.Generic;

class RoomContainer
{
    static List<GameObject> regularRooms;

    static RoomContainer()
    {
        regularRooms = new List<GameObject>();
        regularRooms.AddRange(Resources.LoadAll<GameObject>("Level/Rooms/Regular"));
    }

    public static GameObject GetRegularRoomInstance()
    {
        return Object.Instantiate(regularRooms[Random.Range(0, regularRooms.Count)]);
    }
}