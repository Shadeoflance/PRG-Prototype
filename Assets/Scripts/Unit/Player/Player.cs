using UnityEngine;
using System.Collections;

public class Player : Unit
{
    public BoxCollider2D top, bot, left, right;
    public float jumpForce, jumpHeight;
	void Start()
	{
		controller = new PlayerController(this);
        jumper = new MultipleJumper(this, jumpForce, jumpHeight, 2);
        mover = new DefaultMover(this, speed);
        state = new PlayerWalkingState(this);
        eventManager.SubscribeHandler("land", new WalkOnLand());
        eventManager.SubscribeHandler("jumpButtonDown", new JumpInvoker());
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            return;
        foreach (var a in collision.contacts)
        {
            if (a.otherCollider == bot)
            {
                eventManager.InvokeHandlers("land");
                break;
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (!bot.IsTouchingLayers(1 << LayerMask.NameToLayer("Level")) && !(state is PlayerAirborneState))
        {
            state.Transit(new PlayerAirborneState(this).Enter());
        }
        Vector2 needVel = controller.NeedVel();
        if (needVel.magnitude > 0)
        {
            state.Move(needVel);
        }
    }

    class WalkOnLand : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.state.Transit(new PlayerWalkingState(u).Enter());
            return false;
        }
    }
    class JumpInvoker : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.state.Jump();
            return false;
        }
    }
}
