using System;
using UnityEngine;

public class Enemy : Unit
{
    protected virtual void Start()
    {
        eventManager.SubscribeHandler("takeDmg", new DmgTextCreate());
        eventManager.SubscribeHandler("takeDmg", new DmgRedPaint());
        eventManager.SubscribeHandler("die", new RoomDieInfo());
        eventManager.SubscribeHandler("die", new PickupsDrop());
    }
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            attack.DealDamage(collision.gameObject.GetComponent<Player>());
        }
    }

    class DmgTextCreate : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            float amount = (float)ap["amount"];
            amount = (float)Math.Round(amount, 1);
            Color inColor = new Color(1f, 0.9f, 0f);
            Color outColor = new Color(1f, 0.4f, 0.3f);
            DamageText.Create(ap.unit.transform.position, amount.ToString(), inColor, outColor);
            return false;
        }
    }
    class DmgRedPaint : ActionListener
    {
        SpritePainter painter;
        public bool Handle(ActionParams ap)
        {
            if (painter == null)
                painter = ap.unit.transform.FindChild("Sprite").GetComponent<SpritePainter>();
            painter.Paint(Color.red, 0.5f, true);
            return false;
        }
    }
    class RoomDieInfo : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            Level.instance.current.EnemyDied((Enemy)ap.unit);
            return true;
        }
    }
    class PickupsDrop : ActionListener
    {
        public int amount = 2;
        public bool Handle(ActionParams ap)
        {
            Gold.Drop(amount, ap.unit.transform.position);
            OrbPickup.Drop(1, ap.unit.transform.position);
            return false;
        }
    }
}