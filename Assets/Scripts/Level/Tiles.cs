using UnityEngine;
using System.Collections.Generic;
using System;

class Tiles : MonoBehaviour
{
    Tile[,] map = new Tile[(int)Level.roomSize.x * 2, (int)Level.roomSize.y * 2];

    void Awake()
    {
        foreach(Transform a in transform)
        {
            if(a != null)
            {
                Tile t = a.GetComponent<Tile>();
                int i = (int)((a.localPosition.x + Level.roomSize.x / 2 - 0.25f) / 0.5f);
                int j = (int)((a.localPosition.y + Level.roomSize.y / 2 - 0.25f) / 0.5f);
                try
                {
                    map[i, j] = t;
                } catch(IndexOutOfRangeException)
                {
                    Debug.LogError(i + " " + j);
                }
                t.processed = false;
                t.x = i;
                t.y = j;
            }
        }
        UpdateColliders();
    }

    void UpdateColliders()
    {
        for (int y = 0; y < map.GetLength(1); y++)
        {
            Tile t = null;
            int k = 1;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if(t == null && map[x, y] != null)
                {
                    t = map[x, y];
                    continue;
                }
                else if (t != null && map[x, y] != null)
                    k++;
                else if(t != null && map[x, y] == null)
                {
                    BoxTiles(t, k);
                    k = 1;
                    t = null;
                }
            }
            if (t != null)
                BoxTiles(t, k);
        }
    }

    void BoxTiles(Tile start, int count)
    {
        BoxCollider2D box = start.gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(0.5f * count, 0.5f);
        box.offset = new Vector2(box.size.x / 2 - 0.25f, 0);
    }
}