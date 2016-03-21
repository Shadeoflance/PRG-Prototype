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
        items.Add(1, new Bundle(Resources.Load<Sprite>("Items/extrajump"), (player) =>
            {
                if (player.jumper is MultipleJumper)
                {
                    MultipleJumper jumper = (MultipleJumper)player.jumper;
                    jumper.AddExtraJumps(1);
                }
                else if (player.jumper is DefaultJumper)
                {
                    DefaultJumper jumper = (DefaultJumper)player.jumper;
                    player.jumper = new MultipleJumper(player, jumper.force, jumper.maxHeight, 1);
                }
            }
        ));

        items.Add(2, new Bundle(Resources.Load<Sprite>("Items/extradmg"), (player) =>
            {
                player.attack.dmgUps++;
            }
        ));

        items.Add(3, new Bundle(Resources.Load<Sprite>("Items/dmgafterdash"), (player) =>
            {
                player.eventManager.SubscribeHandler("dashFinish", new DmgAfterDash());
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

}