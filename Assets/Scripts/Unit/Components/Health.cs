using System;
using UnityEngine;

public class Health
{
    Unit unit;
    public float currentHealth, maxHealth;
    public Health(Unit unit, float maxHealth)
    {
        this.unit = unit;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
        unit.eventManager.SubscribeHandler("takeDmg", new Knockback(unit is Player));
    }

    public void TakeDamage(float amount, GameObject source)
    {
        ActionParams ap = new ActionParams();
        ap["amount"] = amount;
        ap["source"] = source;
        unit.eventManager.InvokeInterceptors("takeDmg", ap);
        if (ap.forbid)
            return; 
        currentHealth -= (float)ap["amount"];
        unit.eventManager.InvokeHandlers("takeDmg", ap);
        if (currentHealth <= 0)
        {
            Die();
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
        unit.eventManager.InvokeHandlers("die", null);
        //GameObject.Destroy(unit.gameObject);
        unit.gameObject.SetActive(false);
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