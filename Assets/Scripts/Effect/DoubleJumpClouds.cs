﻿using UnityEngine;

public static class DoubleJumpClouds
{
    static Prefab prefab = new Prefab("Effects/DoubleJump");
    public static void Create(Vector2 position)
    {
        var newInstance = prefab.Instantiate();
        newInstance.transform.position = position;
        var ps = newInstance.GetComponent<ParticleSystem>();
        ps.Play();
        Object.Destroy(newInstance, ps.duration);
    }
}