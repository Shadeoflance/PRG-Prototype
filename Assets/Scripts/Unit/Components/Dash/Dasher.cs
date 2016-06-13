using UnityEngine;
using UnityEngine.UI;

public class Dasher : IUpdatable
{
    public float coolDown = 5;
    private float currentCoolDown = 0;
    protected Player player;
    public Dasher(Player player)
    {
        this.player = player;
    }

    public virtual void Dash()
    {
        currentCoolDown = coolDown;
    }

    public virtual void Update()
    {
        if (currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
            if (currentCoolDown < 0)
                currentCoolDown = 0;
        }
    }

    protected virtual bool CanDash()
    {
        return currentCoolDown <= 0;
    }
}