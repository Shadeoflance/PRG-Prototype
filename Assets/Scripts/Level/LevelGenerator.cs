using UnityEngine;

class LevelGenerator
{
    public static void Generate(Map map)
    {
        map[map.size / 2, map.size / 2] = RoomContainer.GetSpawnInstance();
        CreateShop(map);
    }
    static void CreateShop(Map map)
    {
        Vector2 v = RandOnCircle(Utils.CoinInt(), map);
        map[(int)v.x, (int)v.y] = RoomContainer.GetShopInstance();
        CreatePath(map, (int)v.x, (int)v.y);
    }
    static void CreatePath(Map map, int x, int y)
    {
        Vector2 v = (new Vector2(map.size / 2, map.size / 2) - new Vector2(x, y)).OneNormalize();
        while (true)
        {
            Debug.LogWarning(x + " " + y);
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