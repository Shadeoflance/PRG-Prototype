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
    }

    public void TakeDamage(float amount, GameObject source)
    {
        ActionParams ap = new ActionParams();
        ap["amount"] = amount;
        unit.eventManager.InvokeInterceptors("takeDmg", ap);
        if (ap.forbid)
            return; 
        currentHealth -= (float)ap["amount"];
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