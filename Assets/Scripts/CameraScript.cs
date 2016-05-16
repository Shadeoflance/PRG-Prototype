using UnityEngine;
using System;

class CameraScript : MonoBehaviour
{
    Player p;
    public static CameraScript instance;
    Camera cam;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        cam = GetComponent<Camera>();
        cam.orthographicSize = (Level.roomSize.x / 2 + 0.5f) / cam.aspect;
    }
    void Start()
    {
        p = Player.instance;
    }

    float windowSize = 1, bx1, by1, bx2, by2;
    void Update()
    {
        float px = p.transform.position.x, py = p.transform.position.y, cx = transform.position.x, cy = transform.position.y;
        float x = Math.Abs(cx - px) < windowSize ?
            cx : (px < cx ? px + windowSize : px - windowSize);
        float y = py;
        y = y < by1 ? by1 : (y > by2 ? by2 : y);
        x = x < bx1 ? bx1 : (x > bx2 ? bx2 : x);
        transform.position = new Vector3(x, y, transform.position.z);
    }
    public void RefreshBorders()
    {
        bx1 = by1 = 99999;
        bx2 = by2 = -99999;
        foreach(var a in Level.instance.current.subRooms)
        {
            bx1 = Math.Min(bx1, a.transform.position.x - Level.roomSize.x / 2 - 0.5f + cam.orthographicSize * cam.aspect);
            bx2 = Math.Max(bx2, a.transform.position.x + Level.roomSize.x / 2 + 0.5f - cam.orthographicSize * cam.aspect);
            by1 = Math.Min(by1, a.transform.position.y - Level.roomSize.y / 2 - 0.5f + cam.orthographicSize);
            by2 = Math.Max(by2, a.transform.position.y + Level.roomSize.y / 2 + 0.5f - cam.orthographicSize);
        }
    }
    public static Vector3 GetPositionToSide(SubRoom subRoom, Vector2 dir)
    {
        Vector2 result = subRoom.transform.position;
        result = result + Vector2.Scale(dir, 
            new Vector2(Level.roomSize.x / 2 + 0.5f - instance.cam.orthographicSize * instance.cam.aspect, 
            Level.roomSize.y / 2 + 0.5f - instance.cam.orthographicSize));
        return result.ToV3() + new Vector3(0, 0, -1);
    }
}