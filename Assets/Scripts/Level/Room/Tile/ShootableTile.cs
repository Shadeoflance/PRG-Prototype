using UnityEngine;

class ShootableTile : Tile
{
    void Start()
    {
        GetComponent<SpritePainter>().Paint(new PulsingChanger(Color.white, new Color(1f, 1f, 0.7f), null, 0.3f));
    }
    public override void BulletHit(Bullet bullet)
    {
        if (bullet.unit is Player)
        {
            Destroy();
        }
    }
}