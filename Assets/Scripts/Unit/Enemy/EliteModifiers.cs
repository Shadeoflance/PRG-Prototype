using UnityEngine;
using System.Collections.Generic;
using System;

static class EliteModifiers
{
    static List<Action<Enemy, SpriteRenderer>> modifiers = new List<Action<Enemy, SpriteRenderer>>();

    static EliteModifiers()
    {
        modifiers.Add((Enemy e, SpriteRenderer sr) =>
        {
            sr.color = new Color(0.58f, 0, 0.17f, 0.5f);
            e.health.maxHealth *= 2;
            e.health.currentHealth = e.health.maxHealth;
        });
        modifiers.Add((Enemy e, SpriteRenderer sr) =>
        {
            sr.color = new Color(0.58f, 0.17f, 0.5f, 0.5f);
            e.eventManager.UnsibscribeHandler("takeDmg", typeof(Knockback));
            e.eventManager.SubscribeHandler("takeDmg", new DamageBoost(0.7f));
        });
        modifiers.Add((Enemy e, SpriteRenderer sr) =>
        {
            sr.color = new Color(0f, 0f, 0f, 0.5f);
            e.attack.baseDmg *= 2;
        });
    }

    public static void SetRandomModifier(Enemy e)
    {
        modifiers[UnityEngine.Random.Range(0, modifiers.Count)].Invoke(e, AddOutline(e));
    }

    static SpriteRenderer AddOutline(Enemy e)
    {
        GameObject ol = new GameObject("Outline");
        SpriteRenderer sr = ol.AddComponent<SpriteRenderer>();
        ol.transform.SetParent(e.transform);
        ol.transform.localPosition = Vector3.zero;
        sr.sprite = e.sr.sprite;
        sr.flipX = e.sr.flipX;
        sr.flipY = e.sr.flipY;
        if(sr.sprite.name == "triangle")
            ol.transform.localScale = new Vector3(1.5f, 1.5f);
        else ol.transform.localScale = new Vector3(1.3f, 1.3f);
        sr.sortingOrder = 0;
        return sr;
    }
}