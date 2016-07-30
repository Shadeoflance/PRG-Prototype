using UnityEngine;

class Shop : SubRoom
{
    public override Color RoomColor
    {
        get
        {
            return Color.yellow;
        }
    }
    protected override void Start()
    {
        isHostile = false;
        base.Start();
    }
}