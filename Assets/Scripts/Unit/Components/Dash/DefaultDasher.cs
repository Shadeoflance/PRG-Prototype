using UnityEngine;
using System.Collections.Generic;

public class DefaultDasher : Dasher
{
    public float speed, distance, dmgMultiplier;
    public DefaultDasher(Player p, float dmgMultiplier = 2.5f, float speed = 50, float distance = 5) : base(p) 
    {
        this.speed = speed;
        this.distance = distance;
        this.dmgMultiplier = dmgMultiplier;
    }

    public override void Dash()
    {
        if (!CanDash())
            return;
        base.Dash();
        if (player.currentState.Transit(new DashState(player, speed, distance)))
        {
            EnemyPenetrate ep = new EnemyPenetrate(player, dmgMultiplier);
            player.eventManager.SubscribeHandler("dashPenetrateEnemy", ep);
            player.eventManager.SubscribeHandler("dashFinish", new EPUnsubscriber(ep));
        }
    }

    class EnemyPenetrate : ActionListener
    {
        HashSet<Enemy> alreadyPenetrated = new HashSet<Enemy>();
        Player player;
        float dmgMul;
        public EnemyPenetrate(Player p, float dmgMul)
        {
            player = p;
            this.dmgMul = dmgMul;
        }

        public bool Handle(ActionParams ap)
        {
            Enemy enemy = (Enemy)ap["enemy"];
            if (!alreadyPenetrated.Contains(enemy))
            {
                player.attack.DealDamage(enemy, dmgMul);
                alreadyPenetrated.Add(enemy);
            }
            return false;
        }
    }

    class EPUnsubscriber : ActionListener
    {
        EnemyPenetrate ep;
        public EPUnsubscriber(EnemyPenetrate ep)
        {
            this.ep = ep;
        }
        public bool Handle(ActionParams ap)
        {
            ap.unit.eventManager.UnsubscribeHandler("dashPenetrateEnemy", ep);
            return true;
        }
    }
}