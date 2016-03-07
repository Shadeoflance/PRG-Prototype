using UnityEngine;

class MultipleJumper : DefaultJumper
{
    public int extraJumps;
    int currentJumps;
    class JumpsRefresher : EventHandler
    {
        public bool Handle(Unit u)
        {
            (u.jumper as MultipleJumper).RefreshJumps();
            return false;
        }
    }
    public MultipleJumper(Unit unit, float force, float maxHeight, int extraJumps = 0) : base(unit, force, maxHeight) 
    {
        this.extraJumps = extraJumps;
        currentJumps = extraJumps;
        unit.eventManager.SubscribeHandler("land", new JumpsRefresher());
    }

    public void RefreshJumps()
    {
        currentJumps = extraJumps;
    }

    public void AddExtraJumps(int amount)
    {
        extraJumps += amount;
        if (unit.currentState != unit.airborne)
            currentJumps = extraJumps;
    }

    protected override bool CanJump()
    {
        if (!(unit.currentState == unit.airborne))
            return true;
        if (currentJumps > 0)
        {
            currentJumps--;
            return true;
        }
        return base.CanJump();
    }
}