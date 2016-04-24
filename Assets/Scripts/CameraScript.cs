using UnityEngine;
using System;

class CameraScript : MonoBehaviour
{
    Player p;
    public static CameraScript instance;
    Camera camera;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        camera = GetComponent<Camera>();
        camera.orthographicSize = Level.roomSize.x / 2 / camera.aspect;
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
            bx1 = Math.Min(bx1, a.transform.position.x - Level.roomSize.x / 2 + camera.orthographicSize * camera.aspect);
            bx2 = Math.Max(bx2, a.transform.position.x + Level.roomSize.x / 2 - camera.orthographicSize * camera.aspect);
            by1 = Math.Min(by1, a.transform.position.y - Level.roomSize.y / 2 + camera.orthographicSize);
            by2 = Math.Max(by2, a.transform.position.y + Level.roomSize.y / 2 - camera.orthographicSize);
        }
        Debug.LogWarning(camera.orthographicSize);
        Debug.LogWarning(bx1 + " " + by1 + " " + bx2 + " " + by2);
    }
}