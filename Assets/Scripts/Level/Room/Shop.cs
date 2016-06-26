using UnityEngine;

class Shop : SubRoom
{
    protected override void Start()
    {
        isHostile = false;
        base.Start();
    }
}