using System;
using UnityEngine;

public class Attack : IUpdatable
{
    protected Unit unit;
    public float baseDmg, dmgUps = 0, flatDmg = 0;
    public Attack(Unit unit)
    {
        this.unit = unit;
        this.baseDmg = unit.damage;
    }
    public virtual void DoAttack() { }

    public virtual void DealDamage(Unit victim, float multiplier = 1)
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

    public virtual void Update()
    {
    }
}