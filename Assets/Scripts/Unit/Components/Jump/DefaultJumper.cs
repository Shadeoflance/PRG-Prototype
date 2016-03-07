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
    class JumpUp : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.rb.gravityScale = ap.unit.gravityScale;
            return true;
        }
    }
    public override void Jump()
    {
        if (!CanJump())
            return;
        ActionParams ap = new ActionParams();
        unit.eventManager.InvokeInterceptors("jump", ap);
        if (ap.forbid)
            return;
        unit.currentState.Transit(unit.airborne);
        unit.eventManager.InvokeHandlers("jump", null);
        unit.rb.velocity = new Vector2(unit.rb.velocity.x, force);
        unit.rb.gravityScale = 0;
        unit.eventManager.SubscribeHandlerWithTimeTrigger("jumpButtonUp", new JumpUp(), maxHeight);
    }
    protected override bool CanJump()
    {
        return !(unit.currentState == unit.airborne);
    }
}