using UnityEngine;

class Spawn : Room
{
    public override Color RoomColor
    {
        get
        {
            return Color.blue;
        }
    }
    protected override void Start()
    {
        isHostile = false;
        base.Start();
    }
}