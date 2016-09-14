using UnityEngine;

public class BulletFactory
{
    Bullet bullet;
    Sprite sprite;
    Vector2 direction, position;
    Color color = Color.white;
    Unit player;
    float speed;
    float? life;
    float dmgMult = 1;
    int dmgMask;
    Group<BulletProcessingModifier> modifiers = new Group<BulletProcessingModifier>();

    public BulletFactory(Unit player)
    {
        this.player = player;
    }

    public BulletFactory SetSprite(Sprite sprite)
    {
        this.sprite = sprite;
        return this;
    }

    public BulletFactory SetBullet(Bullet bullet)
    {
        this.bullet = bullet;
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

    public BulletFactory SetColor(Color color)
    {
        this.color = color;
        return this;
    }

    public BulletFactory SetLife(float life)
    {
        this.life = life;
        return this;
    }

    public BulletFactory SetDmgMult(float dmgMult)
    {
        this.dmgMult = dmgMult;
        return this;
    }

    public BulletFactory AddModifier(BulletProcessingModifier mod)
    {
        modifiers.Add(mod);
        return this;
    }

    public BulletFactory SetDmgMask(int mask)
    {
        dmgMask = mask;
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
        newBullet.dmgMult = dmgMult;
        newBullet.unit = player;
        newBullet.dmgMask = dmgMask;
        SpriteRenderer sr = newBullet.GetComponent<SpriteRenderer>();
        sr.color = color;
        if (sprite != null)
            sr.sprite = sprite;
        foreach (var a in modifiers)
            newBullet.modifiers.Add(a.Instantiate());
        return newBullet;
    }
}
