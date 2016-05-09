using UnityEngine;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;

struct Bundle
{
    public Bundle(string path, Action action)
    {
        this.sprite = Resources.Load<Sprite>("Items/" + path);
        this.action = action;
    }
    public Sprite sprite;
    public Action action;
}
public static class ItemPool
{
    static List<Bundle> items;
    static Bundle extraDmg;

    public static void AssignItem(Item item)
    {
        Bundle bundle = GetRandom();
        items.Remove(bundle);
        item.getAction = bundle.action;
        item.GetComponent<SpriteRenderer>().sprite = bundle.sprite;
    }

    static Bundle GetRandom()
    {
        if (items.Count == 0)
            return extraDmg;
        return items[Random.Range(0, items.Count)];
    }

    static ItemPool()
    {
        items = new List<Bundle>();
        items.Add(new Bundle("extrajump", () =>
            {
                if (Player.instance.jumper is DefaultJumper)
                {
                    var jumper = (DefaultJumper)Player.instance.jumper;
                    jumper.AddExtraJumps(1);
                }
            }
        ));

        extraDmg = new Bundle("extradmg", () =>
        {
            Player.instance.attack.dmgUps++;
        }
        );
        items.Add(extraDmg);

        items.Add(new Bundle("dmgafterdash", () =>
            {
                Player.instance.eventManager.SubscribeHandler("dashFinish", new DmgAfterDash());
            }
        ));

        items.Add(new Bundle("crit", () =>
            {
                Player.instance.eventManager.SubscribeInterceptor("shoot", new Crit());
            }
        ));

        items.Add(new Bundle("distancedmg", () =>
            {
                (Player.instance.attack as Weapon).factory.AddModifier(new DistanceDmg());
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
            b.transform.FindChild("Sprite").GetComponent<SpriteRenderer>().color = Color.red;
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
            bullet.dmgMult += dist / 7;
            bullet.transform.FindChild("Sprite").transform.localScale += new Vector3(dist / 7, dist / 7, 0);
            prev = bullet.transform.position.ToV2();
        }

        public BulletProcessingModifier Instantiate()
        {
            return new DistanceDmg();
        }
    }

}