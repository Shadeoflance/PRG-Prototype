﻿using UnityEngine;
using System;

class Level : MonoBehaviour
{
    public static Vector2 roomSize = new Vector2(20, 15);
    public Map map;
    public static Level instance;
    public Room current;
    public bool tutorial;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance);
            instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("scene1");
        }
    }

    void Start()
    {
        map = new Map(7);
        LevelGenerator.Generate(map, tutorial);
        map.PostInitialize();
        current = map.spawn;
        current.Enable();
        Player.instance.transform.position = current.transform.position;
        map.UpdateUI();
        CameraScript.instance.RefreshBorders();
        if (tutorial)
            TutorialInit.Init();
    }

    public void ChangeRoom(Vector2 dir, Room room)
    {
        Room next = map.GetRelativeTo(room, dir);
        ActionParams ap = new ActionParams(Player.instance);
        ap["room"] = next;
        ap["prevRoom"] = current;
        Player.instance.eventManager.InvokeHandlers("roomChange", ap);
        RoomChangeEffect.Create(room, next);
        current = next;
        map.UpdateUI();
        CameraScript.instance.RefreshBorders();
    }

    public void ForceChangeRoom(Room next)
    {
        current.Disable();
        next.Enable();
        current = next;
        map.UpdateUI();
        CameraScript.instance.RefreshBorders();
    }
}