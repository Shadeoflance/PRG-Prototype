using UnityEngine;

public class DoubleDamageBuff : Buff
{
    ActionListener al;
    public DoubleDamageBuff(Unit unit, float duration) : base(unit, duration) 
    {
        al = new DoubleDamageInterceptor();
        unit.eventManager.SubscribeInterceptor("shoot", al);
        imagePath = "Buffs/dmgup";
    }

    public override void End()
    {
        base.End();
        unit.eventManager.UnsubscribeInterceptor("shoot", al);
    }

    class DoubleDamageInterceptor : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            var b = ap["bullet"] as Bullet;
            b.dmg = b.dmg * 2;
            b.GetComponent<SpriteRenderer>().color = Color.red;
            return false;
        }
    }
}