using UnityEngine;

public class Slamer
{
    public float coolDown = 5;
    private float currentCoolDown = 0;
    protected Player player;
    public Slamer(Player player)
    {
        this.player = player;
    }

    public virtual void Slam()
    {
        currentCoolDown = coolDown;
    }

    public virtual void Update()
    {
        if (currentCoolDown > 0)
            currentCoolDown -= Time.deltaTime;
    }

    protected virtual bool CanSlam()
    {
        return currentCoolDown <= 0;
    }
}