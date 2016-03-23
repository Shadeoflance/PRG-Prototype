using UnityEngine;

public class BulletFactory
{
    Bullet bullet;
    Vector2 direction, position;
    Unit player;
    float speed;
    float? life;
    float dmg;
    Group<BulletProcessingModifier> modifiers = new Group<BulletProcessingModifier>();

    public BulletFactory(Unit player)
    {
        this.player = player;
    }

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

    public BulletFactory SetDmg(float dmg)
    {
        this.dmg = dmg;
        return this;
    }

    public BulletFactory AddModifier(BulletProcessingModifier mod)
    {
        modifiers.Add(mod);
        return this;
    }

    public Bullet Build()
    {
        modifiers.Refresh();
        Bullet newBullet = GameObject.Instantiate<Bullet>(bullet);
        newBullet.speed = speed;
        newBullet.transform.position = position;
        newBullet.needVel = direction.normalized;
        newBullet.life = life;
        newBullet.dmgMult = dmg;
        newBullet.player = player;
        newBullet.gameObject.SetActive(true);
        foreach (var a in modifiers)
            newBullet.modifiers.Add(a.Instantiate());
        return newBullet;
    }
}
