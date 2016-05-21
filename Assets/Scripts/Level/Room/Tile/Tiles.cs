using UnityEngine;
using System.Collections.Generic;
using System;

class Tiles : MonoBehaviour
{
    public Tile[,] map = new Tile[(int)Level.roomSize.x * 2 + 2, (int)Level.roomSize.y * 2 + 2];

    void Awake()
    {
        foreach(Transform a in transform)
        {
            if(a != null)
            {
                Tile t = a.GetComponent<Tile>();
                int i = (int)((a.localPosition.x + Level.roomSize.x / 2 - 0.25f) / 0.5f) + 1;
                int j = (int)((a.localPosition.y + Level.roomSize.y / 2 - 0.25f) / 0.5f) + 1;
                try
                {
                    map[i, j] = t;
                } catch(IndexOutOfRangeException)
                {
                    Debug.LogError(i + " " + j);
                }
                t.x = i;
                t.y = j;
            }
        }
        UpdateColliders();
    }

    void UpdateColliders()
    {
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            Tile t = null;
            int k = 1;
            for (int x = 1; x < map.GetLength(0) - 1; x++)
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
    void BoxTilesUp(Tile start, int count)
    {
        BoxCollider2D box = start.gameObject.AddComponent<BoxCollider2D>();
        box.size = new Vector2(0.5f, 0.5f * count);
        box.offset = new Vector2(0, box.size.y / 2 - 0.25f);
    }

    Prefab prefab = new Prefab("Level/Tiles/BasicTile");
    Tile CreateTile(int x, int y)
    {
        Tile newInstance = prefab.Instantiate().GetComponent<Tile>();
        newInstance.x = x;
        newInstance.y = y;
        map[x, y] = newInstance;
        newInstance.transform.parent = transform;
        newInstance.transform.localPosition = new Vector3((x - 1) * 0.5f + 0.25f - Level.roomSize.x / 2, (y - 1) * 0.5f + 0.25f - Level.roomSize.y / 2);

        return newInstance;
    }

    public void AddWall(Vector2 dir)
    {
        if (dir.x > 0)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                CreateTile(map.GetLength(0) - 1, i);
            }
            BoxTilesUp(map[map.GetLength(0) - 1, 0], map.GetLength(1));
        }
        else if(dir.x < 0)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                CreateTile(0, i);
            }
            BoxTilesUp(map[0, 0], map.GetLength(1));
        }
        else if(dir.y > 0)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                CreateTile(i, map.GetLength(1) - 1);
            }
            BoxTiles(map[0, map.GetLength(1) - 1], map.GetLength(0));
        }
        else
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                CreateTile(i, 0);
            }
            BoxTiles(map[0, 0], map.GetLength(0));
        }
    }
}