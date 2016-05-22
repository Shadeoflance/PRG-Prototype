using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class SubRoom : MonoBehaviour
{
    public Room room;
    Dictionary<Vector2, Door> doors = new Dictionary<Vector2,Door>();
    public List<Enemy> enemiesAlive = new List<Enemy>();
    public Tiles tiles;
    public bool isHostile = true;

    public virtual void Awake()
    {
        tiles = transform.FindChild("Tiles").GetComponent<Tiles>();
    }

    protected virtual void Start()
    {
        CreateDoors();
        CreateWalls();
        if(isHostile)
            GenerateEnemies("TestEnemy", Random.Range(1, 4));
        foreach (var a in room.subRooms)
            if (a.isHostile)
            {
                DisableDoors();
                break;
            }
    }

    public void CreateWalls()
    {
        foreach (var dir in Map.dirs)
        {
            SubRoom s = Level.instance.map.GetRelativeTo(this, dir);
            if (!room.subRooms.Contains(s))
            {
                tiles.AddWall(dir);
            }
        }
    }

    public void CreateDoors()
    {
        foreach(var dir in Map.dirs)
        {
            SubRoom s = Level.instance.map.GetRelativeTo(this, dir);
            if (s != null && !room.subRooms.Contains(s))
            {
                CreateSingleDoor(dir);
            }
        }
    }
    
    private void CreateSingleDoor(Vector2 dir)
    {
        Transform parent = transform.FindChild("Doors");
        Vector2 doorDistance = new Vector2((Level.roomSize.x - 1) / 2, (Level.roomSize.y - 1) / 2);
        GameObject doorInstance = Instantiate(Resources.Load<GameObject>("Level/Door"));
        doorInstance.name = "Door" + dir;
        doorInstance.transform.position = transform.position + Vector2.Scale(doorDistance, dir).ToV3();
        if(dir == Vector2.down)
            doorInstance.transform.Rotate(0, 0, 270);
        else if(dir == Vector2.left)
            doorInstance.transform.Rotate(0, 0, 180);
        else if(dir == Vector2.up)
            doorInstance.transform.Rotate(0, 0, 90);
        doorInstance.transform.SetParent(parent.transform);
        doors.Add(dir, doorInstance.GetComponent<Door>());
    }

    public virtual void GenerateEnemies(string name, int amount)
    {
        GameObject prefab = Resources.Load<GameObject>("Enemies/" + name);
        for (int i = 0; i < amount; i++)
        {
            Vector2 position = GetTopClearTilePos() + new Vector2(0, 0.4f);
            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(transform);
            instance.transform.position = position;
            enemiesAlive.Add(instance.GetComponent<Enemy>());
        }
    }

    List<Tile> clearTiles;
    private Vector2 GetTopClearTilePos()
    {
        if (clearTiles == null)
        {
            clearTiles = new List<Tile>();
            foreach (var a in tiles.map)
            {
                if (a == null)
                    continue;
                if(a.y < tiles.map.GetLength(1) - 1 && tiles.map[a.x, a.y + 1] == null)
                {
                    clearTiles.Add(a);
                }
            }
        }
        return clearTiles[Random.Range(0, clearTiles.Count)].transform.position;
    }

    public void EnemyDied(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
    }

    public void DoorTouch(Door door)
    {
        foreach (var a in doors.Keys)
        {
            Door d = doors[a];
            if(door == d)
                Level.instance.ChangeRoom(a, this);
        }
    }

    public void Disable()
    {
        foreach (var a in doors.Values)
            if (a != null)
                a.Close();
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void EnableDoors()
    {
        foreach (var a in doors.Values)
            if(a != null)
                a.Enable();
    }

    public void DisableDoors()
    {
        foreach (var a in doors.Values)
            if(a != null)
                a.Disable();
    }

    public void WrapInRoom()
    {
        room = new Room(this);
    }
}