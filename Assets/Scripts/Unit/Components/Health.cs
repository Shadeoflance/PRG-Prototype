using System;
using UnityEngine;

public class Health
{
    Unit unit;
    public Health(Unit unit)
    {
        this.unit = unit;
        unit.eventManager.SubscribeHandler("takeDmg", new Knockback(unit is Player));
    }

    public void TakeDamage(float amount, GameObject source, bool invertBump = false)
    {
        ActionParams ap = new ActionParams();
        ap["amount"] = amount;
        ap["source"] = source;
        ap["invertBump"] = invertBump;
        unit.eventManager.InvokeInterceptors("takeDmg", ap);
        if (ap.forbid)
            return; 
        unit.stats.hp -= (float)ap["amount"];
        unit.eventManager.InvokeHandlers("takeDmg", ap);
        if (unit.stats.hp <= 0)
        {
            Die();
            Unit u = source.GetComponent<Unit>();
            if (u != null)
            {
                ActionParams sourceAp = new ActionParams(u);
                sourceAp["victim"] = unit;
                u.eventManager.InvokeHandlers("kill", sourceAp);
            }
            return;
        }
        int direction = source.transform.position.x < unit.transform.position.x ? 1 : -1;
        unit.direction = -direction;
    }

    public void Die()
    {
        ActionParams ap = new ActionParams();
        ap.unit = unit;
        unit.eventManager.InvokeInterceptors("die", ap);
        if (ap.forbid)
            return;
        unit.gameObject.SetActive(false);
        unit.eventManager.InvokeHandlers("die", null);
    }
}

class Knockback : ActionListener
{
    bool isPlayer;
    public Knockback(bool isPlayer)
    {
        this.isPlayer = isPlayer;
    }
    public bool Handle(ActionParams ap)
    {
        GameObject source = (GameObject)ap["source"];
        int direction = source.transform.position.x < ap.unit.transform.position.x ? 1 : -1;
        if ((bool)ap["invertBump"])
            direction *= -1;
        ap.unit.currentState.Transit(isPlayer ?
            (UnitState)new PlayerDamageTakenState(ap.unit, direction) : new DamageTakenState(ap.unit, direction));
        return false;
    }
}

class DamageBoost : ActionListener
{
    float time;
    public DamageBoost(float time = 1)
    {
        this.time = time;
    }

    public bool Handle(ActionParams ap)
    {
        ap.unit.AddBuff(new Invulnerability(ap.unit, time));
        return false;
    }
}