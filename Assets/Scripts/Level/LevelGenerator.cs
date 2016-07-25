using UnityEngine;
using System.Collections.Generic;

class LevelGenerator
{
    public static void Generate(Map map)
    {
        map[map.size / 2, map.size / 2] = RoomContainer.GetSpawnInstance();
        CreateShop(map);
        CreateRegularFar(map);
        CreateRegularFar(map);
        for (int i = 0; i < 3; i++)
            CreateRegularRandom(map);
    }
    static void CreateShop(Map map)
    {
        Vector2 v = RandOnCircle(Utils.CoinInt(), map);
        map[(int)v.x, (int)v.y] = RoomContainer.GetShopInstance();
        CreatePath(map, (int)v.x, (int)v.y);
    }
    static void CreateRegularFar(Map map)
    {
        Vector2 v = RandOnCircle(Utils.CoinInt(), map);
        map[(int)v.x, (int)v.y] = RoomContainer.GetRegularRoomInstance();
        CreatePath(map, (int)v.x, (int)v.y);
    }
    static void CreateRegularRandom(Map map)
    {
        while(true)
        {
            SubRoom r = map.regulars[Random.Range(0, map.regulars.Count)];
            List<Vector2> dirs = new List<Vector2>(Map.dirs);
            for(int i = 0; i < dirs.Count; i++)
            {
                Vector2 d = dirs[i];
                if(map.GetRelativeTo(r, d) != null || !map.CheckBounds(r.x + (int)d.x, r.y + (int)d.y))
                {
                    dirs.Remove(d);
                    i--;
                }
            }
            if (dirs.Count == 0)
                continue;
            Vector2 dir = dirs[Random.Range(0, dirs.Count)];
            map[r.x + (int)dir.x, r.y + (int)dir.y] = RoomContainer.GetRegularRoomInstance();
            break;
        }
    }
    static void CreatePath(Map map, int x, int y)
    {
        Vector2 v = (new Vector2(map.size / 2, map.size / 2) - new Vector2(x, y)).OneNormalize();
        while (true)
        {
            if(v.x != 0 && v.y != 0)
            {
                if(Utils.Coin())
                    x += (int)v.x;
                else
                    y += (int)v.y;
            }
            else if(v.x != 0)
                x += (int)v.x;
            else
                y += (int)v.y;
            if (map[x, y] != null)
                break;
            map[x, y] = RoomContainer.GetRegularRoomInstance();
            v = (new Vector2(map.size / 2, map.size / 2) - new Vector2(x, y)).OneNormalize();
        }
    }
    static Vector2 RandOnCircle(int borderDistance, Map map)
    {
        Vector2 v;
        do
        {
            v = RandDir();
            int t = map.size / 2 - borderDistance;
            v.Scale(new Vector2(t, t));
            t = Random.Range(-map.size / 2 + borderDistance, map.size / 2 - borderDistance + 1);
            v += new Vector2(v.x == 0 ? t : 0, v.y == 0 ? t : 0);
            v += new Vector2(map.size / 2, map.size / 2);
        }
        while (map[(int)v.x, (int)v.y] != null);
        return v;
    }
    static Vector2 RandDir()
    {
        return Map.dirs[Random.Range(0, 4)];
    }
}