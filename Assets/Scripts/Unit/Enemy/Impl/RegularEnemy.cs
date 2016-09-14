using UnityEngine;

public class RegularEnemy : Enemy
{
    protected override void Start()
    {
        controller = new RegularEnemyController(this);
        mover = new DefaultMover(this);
        health = new Health(this);
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
        Vector2 dir = Utils.TrimY(needPoint - Utils.ToV2(unit.transform.position));
        if (dir.magnitude < 0.05)
            return Vector2.zero;
        return dir.normalized;
    }

    public void Update()
    {
        float dist = Player.Distance(unit.transform.position);
        if (dist < 5)
        {
            needPoint = Player.instance.transform.position;
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 5;
            needPoint = Utils.ToV2(unit.transform.position) + new Vector2((Random.Range(0, 3) == 1 ? -1 : 1) * 3, 0);
        }
    }
}