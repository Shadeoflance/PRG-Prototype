using UnityEngine;

class Spawn : SubRoom
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