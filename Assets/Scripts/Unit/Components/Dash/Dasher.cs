using UnityEngine;
using UnityEngine.UI;

public class Dasher : IUpdatable
{
    public float coolDown = 7;
    private float currentCoolDown = 0;
    protected Player player;
    public Dasher(Player player)
    {
        this.player = player;
    }

    public virtual void Dash()
    {
        currentCoolDown = coolDown;
        ActionParams ap = new ActionParams(player);
        ap["cd"] = coolDown;
        player.eventManager.InvokeHandlers("dashStart", ap);
    }

    public virtual void Update()
    {
        if (currentCoolDown > 0)
        {
            currentCoolDown -= Time.deltaTime;
        }
    }

    protected virtual bool CanDash()
    {
        return currentCoolDown <= 0;
    }
}