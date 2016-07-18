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
            e.sr.color = new Color(0.5f, 0, 0.05f);
            e.health.maxHealth *= 2;
            e.health.currentHealth = e.health.maxHealth;
        });
        modifiers.Add((Enemy e) =>
        {
            e.sr.color = new Color(0.5f, 0.12f, 0.5f);
            e.eventManager.UnsibscribeHandler("takeDmg", typeof(Knockback));
            e.eventManager.SubscribeHandler("takeDmg", new DamageBoost(0.7f));
        });
        modifiers.Add((Enemy e) =>
        {
            e.sr.color = new Color(0.4f, 0.4f, 0.4f);
            e.attack.baseDmg *= 2;
        });
        modifiers.Add((Enemy e) =>
        {
            e.sr.color = new Color(1f, 1f, 0.5f);
            e.eventManager.SubscribeInterceptor("takeDmg", new DamageBlock(0.4f));
        });
    }

    public static void SetRandomModifier(Enemy e)
    {
        modifiers[UnityEngine.Random.Range(0, modifiers.Count)].Invoke(e);
    }

    //static SpriteRenderer AddOutline(Enemy e)
    //{
    //    GameObject ol = new GameObject("Outline");
    //    SpriteRenderer sr = ol.AddComponent<SpriteRenderer>();
    //    ol.transform.SetParent(e.transform);
    //    ol.transform.localPosition = Vector3.zero;
    //    sr.sprite = e.sr.sprite;
    //    sr.flipX = e.sr.flipX;
    //    sr.flipY = e.sr.flipY;
    //    if(sr.sprite.name == "triangle")
    //        ol.transform.localScale = new Vector3(1.5f, 1.5f);
    //    else ol.transform.localScale = new Vector3(1.3f, 1.3f);
    //    sr.sortingOrder = 0;
    //    return sr;
    //}
}

class DamageBlock : ActionListener
{
    float chance;
    public DamageBlock(float chance)
    {
        this.chance = chance;
    }
    public bool Handle(ActionParams ap)
    {
        if(UnityEngine.Random.Range(0f, 1f) < chance)
        {
            ap.forbid = true;
        }
        return false;
    }
}
