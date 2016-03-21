using UnityEngine;

class TestFlyingEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        controller = new TFEController(this);
        mover = new Flyer(this, speed);
        health = new Health(this, 2f);
        walking = new WalkingState(this);
        airborne = new AirborneState(this);
        attack = new Attack(this);
    }
}

class TFEController : IController
{
    Unit unit;

    public TFEController(Unit unit)
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
        if (Player.instance == null)
            return Vector2.zero;
        Vector2 toPlayer = (Player.instance.transform.position - unit.transform.position).ToV2();
        if (toPlayer.magnitude < 4)
            return toPlayer.normalized;
        else return Vector2.zero;
    }

    public void Update()
    {
    }
}