using UnityEngine;

public class Health
{
    Unit unit;
    public int currentHealth, maxHealth;
    public Health(Unit unit, int maxHealth)
    {
        this.unit = unit;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void DealDamage(int amount, GameObject source)
    {
        ActionParams ap = new ActionParams();
        ap["amount"] = amount;
        unit.eventManager.InvokeInterceptors("takeDmg", ap);
        if (ap.forbid)
            return; 
        currentHealth -= (int)ap["amount"];
        unit.eventManager.InvokeHandlers("takeDmg", ap);
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        ActionParams ap = new ActionParams();
        ap.unit = unit;
        unit.eventManager.InvokeInterceptors("die", ap);
        if (ap.forbid)
            return;
        unit.eventManager.InvokeHandlers("die", null);
        MonoBehaviour.Destroy(unit.gameObject);
    }
}