using UnityEngine;

class FlyingEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        controller = new FEController(this);
        mover = new Flyer(this, speed);
        health = new Health(this, 2f);
        walking = new WalkingState(this);
        airborne = new AirborneState(this);
        attack = new ProjAttack(this);
    }
}

class FEController : IController
{
    Unit unit;

    public FEController(Unit unit)
    {
        this.unit = unit;
    }

    public bool NeedAttack()
    {
        return (unit.transform.position - Player.instance.transform.position).magnitude < 5;
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
        if (toPlayer.magnitude < 9)
            return toPlayer.normalized;
        else return Vector2.zero;
    }

    public void Update()
    {
    }
}