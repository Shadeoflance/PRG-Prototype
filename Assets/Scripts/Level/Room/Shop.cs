using UnityEngine;

class Shop : SubRoom
{
    public override Color InitialColor
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