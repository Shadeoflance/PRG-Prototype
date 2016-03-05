using UnityEngine;
using System.Collections;

public class Player : Unit
{
    public float jumpForce, jumpHeight;
	void Start()
	{
		controller = new PlayerController(this);
        jumper = new MultipleJumper(this, jumpForce, jumpHeight, 1);
        mover = new DefaultMover(this, speed);
        attack = new Weapon(this);
        currentState = new PlayerWalkingState(this);
        walking = new PlayerWalkingState(this);
        airborne = new PlayerAirborneState(this);
        eventManager.SubscribeHandler("jumpButtonDown", new JumpInvoker());
        eventManager.SubscribeHandler("attackButtonDown", new AttackInvoker());
	}

    protected override void Update()
    {
        base.Update();
    }

    class JumpInvoker : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.currentState.Jump();
            return false;
        }
    }
    class AttackInvoker : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.currentState.Attack();
            return false;
        }
    }
}
