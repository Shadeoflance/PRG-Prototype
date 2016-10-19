using UnityEngine;
using System.Collections.Generic;

class LevelGenerator
{
    public static void Generate(Map map, bool tutorial = false)
    {
        if(tutorial)
        {
            GenerateTutorial(map);
            return;
        }
        map[map.size / 2, map.size / 2] = RoomContainer.GetSpawnInstance();
        CreateRegularFar(map);
        CreateRegularFar(map);
        CreateShop(map);
        CreateBoss(map);
        for (int i = 0; i < 3; i++)
            CreateRegularRandom(map);
    }

    static void CreateShop(Map map)
    {
        Vector2 v = RandOnCircle(Utils.CoinInt(), map);
        map[(int)v.x, (int)v.y] = RoomContainer.GetShopInstance();
        CreatePath(map, (int)v.x, (int)v.y);
    }
    static void CreateBoss(Map map)
    {
        Vector2 v = RandOnCircle(0, map);
        while (map[(int)v.x, (int)v.y + 1] != null ||
            map[(int)v.x, (int)v.y - 1] != null ||
            map[(int)v.x - 1, (int)v.y] is Shop ||
            map[(int)v.x + 1, (int)v.y] is Shop)
            v = RandOnCircle(0, map);
        map[(int)v.x, (int)v.y] = RoomContainer.GetBossInstance();
        Vector2 t = (new Vector2(map.size / 2, map.size / 2) - new Vector2(v.x, v.y)).OneNormalize();
        if (t.x == 0)
            t.x = Utils.Coin() ? -1 : 1;
        if(map[(int)(v.x + t.x), (int)v.y] == null)
            map[(int)(v.x + t.x), (int)v.y] = RoomContainer.GetRegularRoomInstance();
        CreatePath(map, (int)(v.x + t.x), (int)v.y);
    }
    static void CreateRegularFar(Map map)
    {
        Vector2 v = RandOnCircle(Utils.CoinInt(), map);
        while (map[(int)v.x + 1, (int)v.y] != null ||
                map[(int)v.x - 1, (int)v.y] != null ||
                map[(int)v.x, (int)v.y + 1] != null ||
                map[(int)v.x, (int)v.y - 1] != null)
        {
            v = RandOnCircle(Utils.CoinInt(), map);
        }
            map[(int)v.x, (int)v.y] = RoomContainer.GetRegularRoomInstance();
        CreatePath(map, (int)v.x, (int)v.y);
    }
    static void CreateRegularRandom(Map map)
    {
        while(true)
        {
            Room r = map.regulars[Random.Range(0, map.regulars.Count)];
            List<Vector2> dirs = new List<Vector2>(Map.dirs);
            for(int i = 0; i < dirs.Count; i++)
            {
                Vector2 d = dirs[i];
                if(map.GetRelativeTo(r, d) != null || !map.CheckBounds(r.x + (int)d.x, r.y + (int)d.y))
                {
                    dirs.Remove(d);
                    i--;
                    continue;
                }
                if(map[r.x + (int)d.x, r.y + (int)d.y + 1] is BossRoom ||
                    map[r.x + (int)d.x, r.y + (int)d.y - 1] is BossRoom)
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
            int bx = x, by = y;
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
            if(map[x, y] is Shop)
            {
                if(bx != x)
                {
                    x = bx;
                    y += (int)v.y;
                }
                else
                {
                    y = by;
                    x += (int)v.x;
                }
            }
            if (map[x, y] != null)
            {
                break;
            }
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

    static void GenerateTutorial(Map map)
    {
        List<Room> rooms = RoomContainer.GetTutorial();
        int x = 0, y = map.size / 2;
        map[x, y] = rooms[0];
        map[++x, y] = rooms[1];
        map[x, ++y] = rooms[2];
        map[++x, y] = rooms[3];
        map[++x, y] = rooms[4];
        map[x, --y] = rooms[5];
        map[++x, y] = rooms[6];
    }
}