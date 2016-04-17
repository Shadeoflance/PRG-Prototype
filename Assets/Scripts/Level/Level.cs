using UnityEngine;
using System;

class Level : MonoBehaviour
{
    public static Vector2 roomSize = new Vector2(20, 15);
    public Map map = new Map(2);
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
        map[0, 0] = Instantiate<GameObject>(roomPrefab).GetComponent<SubRoom>();
        map[1, 0] = Instantiate<GameObject>(roomPrefab).GetComponent<SubRoom>();
        current = map[0, 0];
        current.gameObject.SetActive(true);
        Player.instance.transform.position = current.transform.position;
    }

    public void ChangeRoom(Vector2 dir)
    {
        current.gameObject.SetActive(false);
        SubRoom next = map.GetRelativeTo(current, dir);
        current = next;
        next.gameObject.SetActive(true);
        Player.instance.transform.position = next.transform.position + Vector2.Scale(-dir, new Vector2(8f, 6.5f)).ToV3();
    }
}