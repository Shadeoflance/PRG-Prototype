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
        GameObject roomPrefab = Resources.Load<GameObject>("Level/SubRoom");
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (j == 0 && i != 1)
                    continue;
                map[i, j] = Instantiate(roomPrefab).GetComponent<SubRoom>();
            }
        }
        map[1, 2].room.Unite(map[2, 2].room);
        map[3, 3].room.Unite(map[3, 2].room);
        map.PostInitialize();
        current = map[1, 1].room;
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