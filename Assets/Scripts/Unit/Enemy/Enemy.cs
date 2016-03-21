using System;
using UnityEngine;

public class Enemy : Unit
{
    protected virtual void Start()
    {
        eventManager.SubscribeHandler("takeDmg", new DmgTextCreate());
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
            DamageText.Create(ap.unit.transform.position, amount.ToString());
            return false;
        }
    }
}