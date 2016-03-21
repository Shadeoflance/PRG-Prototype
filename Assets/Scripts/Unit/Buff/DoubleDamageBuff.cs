using UnityEngine;

public class DoubleDamageBuff : Buff
{
    ActionListener al;
    public DoubleDamageBuff(Unit unit, float duration) : base(unit, duration) 
    {
        al = new DoubleDamageInterceptor();
        unit.eventManager.SubscribeInterceptor("dealDamage", al);
        imagePath = "Buffs/dmgup";
    }

    public override void End()
    {
        base.End();
        unit.eventManager.UnsubscribeInterceptor("dealDamage", al);
    }

    class DoubleDamageInterceptor : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap["dmg"] = ((float)ap["dmg"]) * 2;
            return false;
        }
    }
}