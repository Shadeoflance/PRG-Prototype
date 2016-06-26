using UnityEngine;

public class RegularEnemy : Enemy
{
    public bool stationary = false;
    protected override void Start()
    {
        controller = new RegularEnemyController(this);
        mover = new DefaultMover(this, speed);
        health = new Health(this, hp);
        walking = new WalkingState(this);
        airborne = new AirborneState(this);
        attack = new Attack(this);
        base.Start();
    }
}

class RegularEnemyController : IController
{
    float timer = 1;
    RegularEnemy unit;
    Vector2 needPoint;

    public RegularEnemyController(RegularEnemy unit)
    {
        this.unit = unit;
    }

    public bool NeedAttack()
    {
        return false;
    }

    public bool NeedJump()
    {
        return false;
    }

    public Vector2 NeedVel()
    {
        if (unit.stationary)
            return Vector2.zero;
        Vector2 dir = Utils.TrimY(needPoint - Utils.ToV2(unit.transform.position));
        if (dir.magnitude < 0.05)
            return Vector2.zero;
        return dir.normalized;
    }

    public void Update()
    {
        Vector2 toPlayer = Utils.ToV2(Player.instance.transform.position - unit.transform.position);
        if (toPlayer.magnitude < 5)
        {
            needPoint = Player.instance.transform.position;
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 5;
            needPoint = Utils.ToV2(unit.transform.position) + new Vector2((Random.Range(0, 2) == 1 ? -1 : 1) * 3, 0);
        }
    }
}