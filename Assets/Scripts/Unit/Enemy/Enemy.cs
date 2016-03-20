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
            collision.gameObject.GetComponent<Player>().currentState.DealDamage(damage, gameObject);
        }
    }

    class DmgTextCreate : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            DamageText.Create(ap.unit.transform.position, ap["amount"].ToString());
            return false;
        }
    }
}