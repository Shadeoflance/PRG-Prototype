using UnityEngine;

class DefaultJumper : Jumper
{
    public float force;
    public float maxHeight;
    public DefaultJumper(Unit unit, float force, float maxHeight) : base(unit) 
    {
        this.force = force;
        this.maxHeight = maxHeight;
    }
    class JumpUp : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.rb.gravityScale = 5;
            return true;
        }
    }
    public override void Jump()
    {
        if (!CanJump())
            return;
        ActionParams ep = new ActionParams();
        unit.eventManager.InvokeInterceptors("jump", ep);
        if (ep.forbid)
            return;
        unit.currentState.Transit(unit.airborne);
        unit.eventManager.InvokeHandlers("jump");
        unit.rb.velocity = new Vector2(unit.rb.velocity.x, force);
        unit.rb.gravityScale = 0;
        unit.eventManager.SubscribeHandlerWithTimeTrigger("jumpButtonUp", new JumpUp(), maxHeight);
    }
    protected override bool CanJump()
    {
        return !(unit.currentState == unit.airborne);
    }
}