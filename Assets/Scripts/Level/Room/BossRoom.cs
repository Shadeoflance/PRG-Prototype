using UnityEngine;

class BossRoom : SubRoom
{
    public override Color InitialColor
    {
        get
        {
            return Color.red;
        }
    }
    protected override void Start()
    {
        isHostile = false;
        base.Start();
    }
}