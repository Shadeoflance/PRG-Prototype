﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class SubRoom : MonoBehaviour
{
    [System.NonSerialized]
    public Room room;
    public int x, y;
    Dictionary<Vector2, Door> doors = new Dictionary<Vector2,Door>();
    [System.NonSerialized]
    public List<Enemy> enemiesAlive = new List<Enemy>();
    [System.NonSerialized]
    public Tiles tiles;
    public Transform enemies;
    public bool isHostile = true;

    public virtual void Awake()
    {
        tiles = transform.FindChild("Tiles").GetComponent<Tiles>();
    }

    protected virtual void Start()
    {
        CreateDoors();
        CreateWalls();
        if (isHostile)
            InitEnemies();
        foreach (var a in room.subRooms)
            if (a.isHostile && enemiesAlive.Count > 0)
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
        Vector2 doorDistance = new Vector2((Level.roomSize.x) / 2 - 0.3f, (Level.roomSize.y - 0.3f) / 2);
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

    public virtual void InitEnemies()
    {
        foreach (Transform a in enemies)
        {
            enemiesAlive.Add(a.GetComponent<Enemy>());
        }
        //GenerateEnemies("RegularEnemy", Random.Range(1, 4));
        //GenerateEnemies("FlyingEnemy", Random.Range(0, 3), false);
        //GenerateEnemies("DiggerEnemy", Random.Range(1, 3));
        //GenerateEnemies("TeleporterEnemy", Random.Range(0, 2));
        //GenerateEnemies("JumperEnemy", Random.Range(1, 3));
    }

    private void GenerateEnemies(string name, int amount, bool ground = true)
    {
        GameObject prefab = Resources.Load<GameObject>("Enemies/" + name);
        for (int i = 0; i < amount; i++)
        {
            Vector2 position;
            if (ground)
                position = GetTopClearTilePos() + new Vector2(0, 0.4f);
            else position = GetAirClearTilePos();
            GameObject instance = Instantiate(prefab);
            instance.transform.SetParent(enemies);
            instance.transform.position = position;
            enemiesAlive.Add(instance.GetComponent<Enemy>());
        }
    }

    List<Vector2> clearPositions;
    public Vector2 GetTopClearTilePos()
    {
        if (clearPositions == null)
        {
            clearPositions = new List<Vector2>();
            foreach (var a in tiles.map)
            {
                if (a == null)
                    continue;
                if(a.y < tiles.map.GetLength(1) - 1 && tiles.map[a.x, a.y + 1] == null)
                {
                    clearPositions.Add(a.transform.position);
                }
            }
        }
        return clearPositions[Random.Range(0, clearPositions.Count)];
    }

    List<Vector2> clearAirPositions;

    public virtual Color InitialColor
    {
        get
        {
            return Color.white;
        }
    }

    public Vector2 GetAirClearTilePos()
    {
        if(clearAirPositions == null)
        {
            clearAirPositions = new List<Vector2>();
            for (int i = 2; i < tiles.map.GetLength(0) - 2; i++)
                for (int j = 2; j < tiles.map.GetLength(1) - 2; j++)
                {
                    Vector2 v = tiles.GetPosition(i, j);
                    Tile a = tiles.map[i, j];
                    if (a == null || tiles.map[a.x, a.y - 1] == null)
                    {
                        clearAirPositions.Add(v);
                    }
                }
        }
        return clearAirPositions[Random.Range(0, clearAirPositions.Count)];
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