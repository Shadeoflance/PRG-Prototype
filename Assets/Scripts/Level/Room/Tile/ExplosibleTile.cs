using UnityEngine;

class ExplosibleTile : Tile
{
    public override void ExplosionHit()
    {
        Destroy();
    }
}