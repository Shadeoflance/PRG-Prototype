using UnityEngine;
using System.Collections.Generic;

public class DefaultSlamer : Slamer
{
    public float speed, range;
    public DefaultSlamer(Player p, float speed = 50, float range = 2)
        : base(p)
    {
        this.speed = speed;
        this.range = range;
    }

    public override void Slam()
    {
        if (!CanSlam())
            return;
        base.Slam();
        player.currentState.Transit(new SlamState(player, speed, range));
        player.eventManager.SubscribeHandler("slamFinish", new SlamDmg(player));
    }

    class SlamDmg : ActionListener
    {
        Player player;
        public SlamDmg(Player p)
        {
            player = p;
        }
        public bool Handle(ActionParams ap)
        {
            foreach (var a in (IEnumerable<Enemy>)ap["enemies"])
            {
                if (a == null)
                    continue;
                a.health.DealDamage(player.damage, player.gameObject);
            }
            return true;
        }
    }
}