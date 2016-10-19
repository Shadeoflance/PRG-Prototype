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
        generateEnemies = false;
        base.Start();
    }
}