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
        state = new WalkingState(this);
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
        if (!bot.IsTouchingLayers(1 << LayerMask.NameToLayer("Level")) && !(state is AirborneState))
        {
            state.Transit(new AirborneState(this).Enter());
        }
    }

    class WalkOnLand : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.state.Transit(new WalkingState(u).Enter());
            return false;
        }
    }
    class JumpInvoker : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.jumper.Jump();
            return false;
        }
    }
}
