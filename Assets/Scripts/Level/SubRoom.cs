using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

class SubRoom : MonoBehaviour
{
    public Room room;
    public GameObject rightW, leftW, topW, botW;
    Dictionary<Vector2, Door> doors = new Dictionary<Vector2,Door>();
    public List<Enemy> enemiesAlive = new List<Enemy>();

    void Awake()
    {
    }

    void Start()
    {
        foreach (Transform a in transform)
            if (a.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                enemiesAlive.Add(a.GetComponent<Enemy>());
        if (enemiesAlive.Count > 0)
            DisableDoors();
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
        Transform doorsParent = transform.FindChild("Doors");
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
        doorInstance.transform.SetParent(doorsParent.transform);
        doors.Add(dir, doorInstance.GetComponent<Door>());
    }

    public void EnemyDied(Enemy enemy)
    {
        Debug.LogWarning("enemy died");
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