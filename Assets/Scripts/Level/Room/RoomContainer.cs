using UnityEngine;
using System.Collections.Generic;

class RoomContainer
{
    static List<GameObject> regularRooms;
    static List<GameObject> shops;
    static List<GameObject> spawns;
    static List<GameObject> bosses;

    static RoomContainer()
    {
        regularRooms = new List<GameObject>();
        regularRooms.AddRange(Resources.LoadAll<GameObject>("Level/Rooms/Regular"));

        shops = new List<GameObject>();
        shops.AddRange(Resources.LoadAll<GameObject>("Level/Rooms/Shop"));

        spawns = new List<GameObject>();
        spawns.AddRange(Resources.LoadAll<GameObject>("Level/Rooms/Spawn"));

        bosses = new List<GameObject>();
        bosses.AddRange(Resources.LoadAll<GameObject>("Level/Rooms/Boss"));
    }

    public static SubRoom GetRegularRoomInstance()
    {
        return Object.Instantiate(regularRooms[Random.Range(0, regularRooms.Count)]).GetComponent<SubRoom>();
    }

    public static SubRoom GetShopInstance()
    {
        return Object.Instantiate(shops[Random.Range(0, shops.Count)]).GetComponent<SubRoom>();
    }

    public static SubRoom GetSpawnInstance()
    {
        return Object.Instantiate(spawns[Random.Range(0, spawns.Count)]).GetComponent<SubRoom>();
    }

    public static SubRoom GetBossInstance()
    {
        return Object.Instantiate(bosses[Random.Range(0, bosses.Count)]).GetComponent<SubRoom>();
    }
}