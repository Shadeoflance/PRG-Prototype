using UnityEngine;
using System;

class Level : MonoBehaviour
{
    public static Vector2 roomSize = new Vector2(20, 15);
    public Map map = new Map(3);
    public static Level instance;
    public SubRoom current;
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
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 0)
                    continue;
                map[i, j] = Instantiate<GameObject>(roomPrefab).GetComponent<SubRoom>();
            }
        }
        map.FixDoors();
        current = map[1, 1];
        current.gameObject.SetActive(true);
        Player.instance.transform.position = current.transform.position;
    }

    public void ChangeRoom(Vector2 dir)
    {
        current.Disable();
        SubRoom next = map.GetRelativeTo(current, dir);
        current = next;
        next.Enable();
        Player.instance.transform.position = next.transform.position + Vector2.Scale(-dir, new Vector2(8f, 5.5f)).ToV3();
    }
}