using UnityEngine;
using UnityEngine.UI;

public class Slamer
{
    public float coolDown = 10;
    private float currentCoolDown = 0;
    protected Player player;
    public Slamer(Player player)
    {
        this.player = player;
    }

    public virtual void Slam()
    {
        currentCoolDown = coolDown;
        ActionParams ap = new ActionParams(player);
        ap["cd"] = coolDown;
        player.eventManager.InvokeHandlers("slamStart", ap);
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

    protected virtual bool CanSlam()
    {
        return currentCoolDown <= 0;
    }
}