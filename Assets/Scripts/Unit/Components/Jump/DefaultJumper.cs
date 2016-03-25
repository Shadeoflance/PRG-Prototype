using UnityEngine;

class DefaultJumper : Jumper
{
    public float force;
    public float maxHeight;
    public int extraJumps;
    int currentJumps;
    public DefaultJumper(Unit unit, float force, float maxHeight, int extraJumps = 0)
        : base(unit) 
    {
        this.force = force;
        this.maxHeight = maxHeight;
        this.extraJumps = extraJumps;
        currentJumps = extraJumps;
        unit.eventManager.SubscribeHandler("land", new JumpsRefresher());
        unit.eventManager.SubscribeHandler("extraJump", new ExtraJumpEffect(unit));
    }
    class JumpUp : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.rb.gravityScale = ap.unit.gravityScale;
            return true;
        }
    }
    class JumpsRefresher : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            if (!(ap.unit.jumper is DefaultJumper))
            {
                return true;
            }
            (ap.unit.jumper as DefaultJumper).RefreshJumps();
            return false;
        }
    }
    class ExtraJumpEffect : ActionListener
    {
        Unit unit;
        public ExtraJumpEffect(Unit u)
        {
            unit = u;
        }
        public bool Handle(ActionParams ap)
        {
            DoubleJumpClouds.Create(ap.unit.transform.position.ToV2() - new Vector2(0, unit.size.y));
            return false;
        }
    }

    public void RefreshJumps()
    {
        currentJumps = extraJumps;
    }

    public void AddExtraJumps(int amount)
    {
        extraJumps += amount;
        if (unit.currentState != unit.airborne)
            currentJumps = extraJumps;
    }

    public override void Jump()
    {
        int cjBefore = currentJumps;
        if (!CanJump())
            return;
        ActionParams ap = new ActionParams();
        unit.eventManager.InvokeInterceptors("jump", ap);
        if (ap.forbid)
            return;
        unit.currentState.Transit(unit.airborne);
        unit.eventManager.InvokeHandlers("jump", null);
        if (cjBefore != currentJumps)
            unit.eventManager.InvokeHandlers("extraJump", null);
        unit.rb.velocity = new Vector2(unit.rb.velocity.x, force);
        unit.rb.gravityScale = 0;
        unit.eventManager.SubscribeHandlerWithTimeTrigger("jumpButtonUp", new JumpUp(), maxHeight);
    }
    protected override bool CanJump()
    {
        if (unit.currentState == unit.walking)
            return true;
        if (currentJumps > 0)
        {
            currentJumps--;
            return true;
        }
        return false;
    }
}