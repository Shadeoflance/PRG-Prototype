using UnityEngine;

public class BulletFactory
{
    Bullet bullet;
    Vector2 direction, position;
    float speed;

    public BulletFactory SetBullet(Bullet bullet)
    {
        this.bullet = bullet;
        bullet.gameObject.SetActive(false);
        return this;
    }

    public BulletFactory SetDir(Vector2 dir)
    {
        direction = dir;
        return this;
    }

    public BulletFactory SetSpeed(float speed)
    {
        this.speed = speed;
        return this;
    }

    public BulletFactory SetPosition(Vector2 position)
    {
        this.position = position;
        return this;
    }

    public Bullet Build()
    {
        Bullet newBullet = GameObject.Instantiate<Bullet>(bullet);
        newBullet.gameObject.SetActive(true);
        newBullet.speed = speed;
        newBullet.transform.position = position;
        newBullet.needVel = direction.normalized;
        return newBullet;
    }
}
