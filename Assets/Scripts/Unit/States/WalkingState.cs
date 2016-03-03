using UnityEngine;

public class PlayerWalkingState : PlayerState
{
    public PlayerWalkingState(Unit unit) : base(unit) { }
}

public class WalkingState : UnitState
{
    public WalkingState(Unit unit) : base(unit) { }
}