using UnityEngine;

public class BulletFactory
{
    Bullet bullet;
    Vector2 direction, position;
    float speed;
    float? life;

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

    public BulletFactory SetLife(float life)
    {
        this.life = life;
        return this;
    }

    public Bullet Build()
    {
        Bullet newBullet = GameObject.Instantiate<Bullet>(bullet);
        newBullet.speed = speed;
        newBullet.transform.position = position;
        newBullet.needVel = direction.normalized;
        newBullet.life = life;
        newBullet.gameObject.SetActive(true);
        return newBullet;
    }
}
