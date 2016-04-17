using UnityEngine;
using System;

class CameraScript : MonoBehaviour
{
    Player p;
    void Start()
    {
        p = Player.instance;
    }

    float windowSize = 1;
    void Update()
    {
        float px = p.transform.position.x, py = p.transform.position.y, cx = transform.position.x, cy = transform.position.y;
        float x = Math.Abs(cx - px) < windowSize ?
            cx : (px < cx ? px + windowSize : px - windowSize);
        float y = py/*Math.Abs(cy - py) < windowSize ?
            cy : (py < cy ? py + windowSize : py - windowSize)*/;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}