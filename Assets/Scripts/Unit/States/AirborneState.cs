using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    public PlayerAirborneState(Unit unit)
        : base(unit)
    {
    }

    public override void Dash()
    {
        player.slamer.Slam();
    }

    public override bool Transit(UnitState state)
    {
        if (state is PlayerAirborneState)
            return false;
        return base.Transit(state);
    }
}

public class AirborneState : UnitState
{
    public AirborneState(Unit unit)
        : base(unit)
    {
    }

    public override bool Transit(UnitState state)
    {
        if (state == unit.airborne)
            return false;
        return base.Transit(state);
    }
}