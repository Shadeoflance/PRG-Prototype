using UnityEngine;

public class PlayerAirborneState : PlayerState
{
    public PlayerAirborneState(Unit unit)
        : base(unit)
    {
    }

    public override void Transit(UnitState state)
    {
        if (state is PlayerAirborneState)
            return;
        base.Transit(state);
    }
}

public class AirborneState : UnitState
{
    public AirborneState(Unit unit)
        : base(unit)
    {
    }

    public override void Transit(UnitState state)
    {
        if (state == unit.airborne)
            return;
        base.Transit(state);
    }
}