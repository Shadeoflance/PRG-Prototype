using UnityEngine;
using System.Collections.Generic;
using System;

static class EliteModifiers
{
    static List<Action<Enemy>> modifiers = new List<Action<Enemy>>();

    static EliteModifiers()
    {
        modifiers.Add((Enemy e) =>
        {
            e.sr.color = new Color(0.58f, 0, 0.17f);
            e.health.maxHealth *= 2;
            e.health.currentHealth = e.health.maxHealth;
        });
    }

    public static void SetRandomModifier(Enemy e)
    {
        modifiers[UnityEngine.Random.Range(0, modifiers.Count)].Invoke(e);
    }
}