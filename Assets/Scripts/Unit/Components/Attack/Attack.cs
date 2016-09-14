using System;
using UnityEngine;

public class Attack : IUpdatable
{
    protected Unit unit;
    public float dmgUps = 0, flatDmg = 0;
    public Attack(Unit unit)
    {
        this.unit = unit;
    }
    public virtual void DoAttack() { }

    public virtual void DealDamage(Unit victim, float multiplier = 1, bool invertBump = false)
    {
        ActionParams ap = new ActionParams();
        ap["victim"] = victim;
        ap["dmg"] = (unit is Player) ? (unit.stats.damage * (float)Math.Sqrt(dmgUps * 1.2f + 1) + flatDmg) * multiplier
            : unit.stats.damage;
        unit.eventManager.InvokeInterceptors("dealDamage", ap);
        if (ap.forbid)
            return;
        victim.currentState.TakeDamage((float) ap["dmg"], unit.gameObject, invertBump);
        unit.eventManager.InvokeHandlers("dealDamage", ap);
    }

    public virtual void Update()
    {
    }
}