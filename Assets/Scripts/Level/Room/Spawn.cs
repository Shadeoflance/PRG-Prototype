using UnityEngine;

class Spawn : SubRoom
{
    protected override void Start()
    {
        isHostile = false;
        base.Start();
    }
    public override void GenerateEnemies(string name, int amount) { }
}