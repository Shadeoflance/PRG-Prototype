using UnityEngine;

class ShootableTile : Tile
{
    public override void BulletHit(Bullet bullet)
    {
        if(bullet.unit is Player)
        {
            Destroy();
        }
    }
}