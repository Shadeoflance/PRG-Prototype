using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

struct Bundle
{
    public Bundle(Sprite sprite, Action<Player> action)
    {
        this.sprite = sprite;
        this.action = action;
    }
    public Bundle(string path, Action<Player> action)
        : this(Resources.Load<Sprite>("Items/" + path), action)
    { }
    public Sprite sprite;
    public Action<Player> action;
}
public static class ItemPool
{
    static Dictionary<int, Bundle> items;

    public static void AssignItem(Item item)
    {
        if (items == null)
            Init();
        AssignItemWithId(item, Random.Range(0, items.Count) + 1);
    }

    public static void AssignItemWithId(Item item, int id)
    {
        if (items == null)
            Init();
        Bundle bundle = items[id];
        item.getAction = bundle.action;
        item.GetComponent<SpriteRenderer>().sprite = bundle.sprite;
        item.id = id;
    }

    static void Init()
    {
        items = new Dictionary<int, Bundle>();
        items.Add(1, new Bundle("extrajump", (player) =>
            {
                if (player.jumper is DefaultJumper)
                {
                    var jumper = (DefaultJumper)player.jumper;
                    jumper.AddExtraJumps(1);
                }
            }
        ));

        items.Add(2, new Bundle("extradmg", (player) =>
            {
                player.attack.dmgUps++;
            }
        ));

        items.Add(3, new Bundle("dmgafterdash", (player) =>
            {
                player.eventManager.SubscribeHandler("dashFinish", new DmgAfterDash());
            }
        ));

        items.Add(4, new Bundle("crit", (player) =>
            {
                player.eventManager.SubscribeInterceptor("shoot", new Crit());
            }
        ));

        items.Add(5, new Bundle("distancedmg", (player) =>
            {
                (player.attack as Weapon).factory.AddModifier(new DistanceDmg());
            }
        ));
    }
    class DmgAfterDash : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.AddBuff(new DoubleDamageBuff(ap.unit, 3));
            return false;
        }
    }

    class Crit : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            if (Random.Range(0f, 1f) > 0.3f)
                return false;
            var b = ap["bullet"] as Bullet;
            b.GetComponent<SpriteRenderer>().color = Color.red;
            b.dmgMult += 2;
            return false;
        }
    }

    class DistanceDmg : BulletProcessingModifier
    {
        Vector2? prev;
        public void Modify(Bullet bullet)
        {
            if (prev == null)
            {
                prev = bullet.transform.position.ToV2();
                return;
            }
            float dist = (prev.Value - bullet.transform.position.ToV2()).magnitude;
            bullet.dmgMult += dist / 5;
            bullet.transform.localScale += new Vector3(dist / 5, dist / 5, 0);
            prev = bullet.transform.position.ToV2();
        }

        public BulletProcessingModifier Instantiate()
        {
            return new DistanceDmg();
        }
    }

}