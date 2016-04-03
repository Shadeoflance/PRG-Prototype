using System;
using UnityEngine;

public class Attack : IUpdatable
{
    protected Unit unit;
    public float baseDmg, dmgUps = 0, flatDmg = 0;
    public Attack(Unit unit, float baseDmg = 1)
    {
        this.unit = unit;
        this.baseDmg = baseDmg;
    }
    public virtual void DoAttack() { }

    public virtual void DealDamage(Unit victim, float multiplier)
    {
        ActionParams ap = new ActionParams();
        ap["victim"] = victim;
        ap["dmg"] = (unit is Player) ? (baseDmg * (float)Math.Sqrt(dmgUps * 1.2f + 1) + flatDmg) * multiplier
            : baseDmg;
        unit.eventManager.InvokeInterceptors("dealDamage", ap);
        if (ap.forbid)
            return;
        victim.currentState.TakeDamage((float) ap["dmg"], unit.gameObject);
        unit.eventManager.InvokeHandlers("dealDamage", ap);
    }
    public virtual void DealDamage(Unit victim)
    {
        DealDamage(victim, 1);
    }

    public virtual void Update()
    {
    }
}