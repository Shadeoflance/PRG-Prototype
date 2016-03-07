﻿using UnityEngine;
using System.Collections;

public class Player : Unit
{
    public Dasher dasher;
	void Start()
	{
		controller = new PlayerController(this);
        jumper = new MultipleJumper(this, jumpForce, jumpHeight, 1);
        mover = new DefaultMover(this, speed);
        attack = new Weapon(this);
        health = new Health(this, 1);
        dasher = new DefaultDasher(this);
        currentState = new PlayerWalkingState(this);
        walking = new PlayerWalkingState(this);
        airborne = new PlayerAirborneState(this);
        eventManager.SubscribeHandler("jumpButtonDown", new JumpInvoker());
        eventManager.SubscribeHandler("attackButtonDown", new AttackInvoker());
        eventManager.SubscribeHandler("dashButtonDown", new DashInvoker());
	}

    protected override void Update()
    {
        base.Update();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            ActionParams ap = new ActionParams();
            ap["enemy"] = collision.GetComponent<Enemy>();
            eventManager.InvokeHandlers("triggerEnterEnemy", ap);
        }
    }

    class JumpInvoker : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.currentState.Jump();
            return false;
        }
    }
    class AttackInvoker : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.currentState.Attack();
            return false;
        }
    }
    class DashInvoker : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            (ap.unit.currentState as PlayerState).Dash();
            return false;
        }
    }
}
