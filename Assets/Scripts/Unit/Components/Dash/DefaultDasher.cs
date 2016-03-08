using UnityEngine;
using System.Collections.Generic;

public class DefaultDasher : Dasher
{
    public float speed, distance;
    public DefaultDasher(Player p, float speed = 50, float distance = 5) : base(p) 
    {
        this.speed = speed;
        this.distance = distance;
    }

    public override void Dash()
    {
        if (player.currentState.Transit(new DashState(player, speed, distance)))
        {
            EnemyPenetrate ep = new EnemyPenetrate();
            player.eventManager.SubscribeHandler("dashPenetrateEnemy", ep);
            player.eventManager.SubscribeHandler("dashFinish", new EPUnsubscriber(ep));
        }
    }

    class EnemyPenetrate : ActionListener
    {
        HashSet<Enemy> alreadyPenetrated = new HashSet<Enemy>();
        public bool Handle(ActionParams ap)
        {
            Enemy enemy = (Enemy)ap["enemy"];
            if (!alreadyPenetrated.Contains(enemy))
            {
                enemy.currentState.Damage(1);
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
            ap.unit.eventManager.UnsubscribeHandler("triggerEnterEnemy", ep);
            return true;
        }
    }
}