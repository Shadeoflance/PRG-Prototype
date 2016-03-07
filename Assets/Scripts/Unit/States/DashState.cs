using UnityEngine;

public class DashState : PlayerState
{
    float speed, distance;
    Vector3 startPoint;
    public DashState(Unit unit, float speed, float distance) : base(unit) 
    {
        this.speed = speed;
        this.distance = distance;
        startPoint = unit.transform.position;
        unit.eventManager.SubscribeHandler("levelSideHit", new WallHitHandler());
        foreach (var a in unit.GetComponents<Collider2D>())
        {
            a.isTrigger = true;
        }
        unit.rb.gravityScale = 0;
    }

    public override void Attack() { }
    public override void Damage(int amount) { }
    public override void Dash() { }
    public override void Jump() { }
    public override void Move(Vector2 dir) { }
    public override bool Transit(UnitState state) 
    {
        return false;
    }
    public override void Slam() { }
    public override void Update()
    {
        unit.rb.velocity = new Vector2(speed * unit.direction, 0);
        if (Vector3.Distance(startPoint, unit.transform.position) > distance)
            Finish();
    }
    public void Finish()
    {
        player.rb.velocity = Vector3.zero;
        player.currentState = player.walking;
        foreach (var a in player.GetComponents<Collider2D>())
        {
            a.isTrigger = false;
        }
        player.rb.gravityScale = player.gravityScale;
        player.eventManager.InvokeHandlers("dashFinish", null);
    }
    class WallHitHandler : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            if (ap.unit.currentState is DashState)
                (ap.unit.currentState as DashState).Finish();
            return true;
        }
    }
}