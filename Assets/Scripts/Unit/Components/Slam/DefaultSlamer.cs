using UnityEngine;
using System.Collections.Generic;

public class DefaultSlamer : Slamer
{
    public float speed, range, dmgMultiplier;
    public DefaultSlamer(Player p, float dmgMultiplier = 3f, float speed = 50, float range = 3)
        : base(p)
    {
        this.speed = speed;
        this.range = range;
        this.dmgMultiplier = dmgMultiplier;
    }

    public override void Slam()
    {
        if (!CanSlam())
            return;
        base.Slam();
        player.currentState.Transit(new SlamState(player, speed, range));
        player.eventManager.SubscribeHandler("slamFinish", new SlamDmg(player, dmgMultiplier));
    }

    class SlamDmg : ActionListener
    {
        Player player;
        float dmgMult;
        public SlamDmg(Player p, float dmgMult)
        {
            player = p;
            this.dmgMult = dmgMult;
        }
        public bool Handle(ActionParams ap)
        {
            foreach (var a in (IEnumerable<Enemy>)ap["enemies"])
            {
                if (a == null)
                    continue;
                ap.unit.attack.DealDamage(a, dmgMult);
            }
            return true;
        }
    }
}