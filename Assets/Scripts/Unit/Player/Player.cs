﻿using UnityEngine;
using System.Collections;

public class Player : Unit
{
    public BoxCollider2D top, bot, left, right;
    public float jumpForce, jumpHeight;
	void Start()
	{
		controller = new PlayerController(this);
        jumper = new DefaultJumper(this, jumpForce, jumpHeight);
        state = new WalkingState(this);
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
    }
}
