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
                    t.tiles = this;
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

    public Vector2 GetPosition(int i, int j)
    {
        return new Vector2((i - 1) * 0.5f + 0.25f - Level.roomSize.x / 2, (j - 1) * 0.5f + 0.25f - Level.roomSize.y / 2) 
            + transform.position.ToV2();
    }

    public void DestroyTile(Tile t)
    {
        Destroy(t.gameObject);
        map[t.x, t.y] = null;
        UpdateColliders();
    }

    void UpdateColliders()
    {
        for (int y = 1; y < map.GetLength(1) - 1; y++)
        {
            Tile t = null;
            int k = 1;
            bool oneway = false;
            for (int x = 1; x < map.GetLength(0) - 1; x++)
            {
                RemoveCollider(map[x, y]);
                if(t == null && map[x, y] != null)
                {
                    t = map[x, y];
                    oneway = t is OneWayTile;
                    continue;
                }
                else if(t != null)
                {
                    if(map[x, y] != null)
                    {
                        if((map[x, y] is OneWayTile) == oneway)
                        {
                            k++;
                        }
                        else
                        {
                            BoxTiles(t, k, false);
                            k = 1;
                            t = null;
                        }
                    }
                    else
                    {
                        BoxTiles(t, k, false);
                        k = 1;
                        t = null;
                    }
                }
            }
            if (t != null)
                BoxTiles(t, k, false);
        }
    }

    void RemoveCollider(Tile t)
    {
        if (t == null)
            return;
        Collider2D[] cols = t.GetComponents<Collider2D>();
        foreach (var a in cols)
            if (!a.isTrigger)
                Destroy(a);
    }

    void BoxTiles(Tile start, int count, bool up)
    {
        BoxCollider2D box = start.gameObject.AddComponent<BoxCollider2D>();
        box.size = up ? new Vector2(0.5f, 0.5f * count) : new Vector2(0.5f * count, 0.5f);
        box.offset = up ? new Vector2(0, box.size.y / 2 - 0.25f) : new Vector2(box.size.x / 2 - 0.25f, 0);
        if(start is OneWayTile)
        {
            box.usedByEffector = true;
        }
    }

    Prefab prefab = new Prefab("Level/Tiles/BasicTile");
    Tile CreateWallTile(int x, int y)
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
                CreateWallTile(map.GetLength(0) - 1, i);
            }
            BoxTiles(map[map.GetLength(0) - 1, 0], map.GetLength(1), true);
        }
        else if(dir.x < 0)
        {
            for (int i = 0; i < map.GetLength(1); i++)
            {
                CreateWallTile(0, i);
            }
            BoxTiles(map[0, 0], map.GetLength(1), true);
        }
        else if(dir.y > 0)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                CreateWallTile(i, map.GetLength(1) - 1);
            }
            BoxTiles(map[0, map.GetLength(1) - 1], map.GetLength(0), false);
        }
        else
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                CreateWallTile(i, 0);
            }
            BoxTiles(map[0, 0], map.GetLength(0), false);
        }
    }
}