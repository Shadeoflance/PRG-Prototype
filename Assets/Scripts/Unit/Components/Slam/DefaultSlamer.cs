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
        player.currentState.Transit(new SlamState(player, speed, range));
        player.eventManager.SubscribeHandler("slamFinish", new SlamDmg());
    }

    class SlamDmg : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            foreach (var a in (List<Enemy>)ap["enemies"])
            {
                if (a == null)
                    continue;
                a.health.TakeDamage(2);
            }
            return true;
        }
    }
}