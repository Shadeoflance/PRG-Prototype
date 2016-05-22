using UnityEngine;
using System;

class Level : MonoBehaviour
{
    public static Vector2 roomSize = new Vector2(20, 15);
    public Map map = new Map(4);
    public static Level instance;
    public Room current;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance);
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (j == 0 && i != 1)
                    continue;
                if (j == 1 && i == 1)
                {
                    map[i, j] = RoomContainer.GetSpawnInstance();
                    continue;
                }   
                //if(i == 2 && j == 1)
                //{
                //    map[i, j] = RoomContainer.GetShopInstance();
                //    continue;
                //}
                map[i, j] = RoomContainer.GetRegularRoomInstance();
                if (UnityEngine.Random.Range(0, 1) > 0.7f)
                    map[i, j].isHostile = false;
            }
        }
        map[1, 2].room.Unite(map[2, 2].room);
        map[3, 3].room.Unite(map[3, 2].room);
        map.PostInitialize();
        current = map.spawn.room;
        current.Enable();
        Player.instance.transform.position = current.subRooms[0].transform.position;
        map.UpdateUI();
        CameraScript.instance.RefreshBorders();
    }

    public void ChangeRoom(Vector2 dir, SubRoom subRoom)
    {
        SubRoom next = map.GetRelativeTo(subRoom, dir);
        RoomChangeEffect.Create(subRoom, next);
        current = next.room;
        map.UpdateUI();
        CameraScript.instance.RefreshBorders();
    }
}