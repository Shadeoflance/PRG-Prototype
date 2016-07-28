using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class Player : Unit
{
    public bool god = false;
    public Dasher dasher;
    public Slamer slamer;
    public BoxCollider2D main;
    public int gold = 0;
    public int orbs = 0;
    public float jumpForce, jumpHeight;
    protected override void Awake()
    {
        base.Awake();
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
    }
    protected override void Start()
    {
		controller = new PlayerController(this);
        jumper = new DefaultJumper(this, jumpForce, jumpHeight, 1);
        mover = new DefaultMover(this, speed);
        attack = new Weapon(this);
        health = new Health(this, hp);
        dasher = new DefaultDasher(this);
        slamer = new DefaultSlamer(this);
        currentState = new PlayerWalkingState(this);
        walking = new PlayerWalkingState(this);
        airborne = new PlayerAirborneState(this);
        eventManager.SubscribeHandler("jumpButtonDown", new JumpInvoker());
        eventManager.SubscribeHandler("dashButtonDown", new DashInvoker());
        eventManager.SubscribeHandler("takeDmg", new DamageBoost());
        eventManager.SubscribeHandler("bombButtonDown", new BombDropInvoker());
        base.Start();
        if(god)
        {
            mover = new Flyer(this, speed);
            gravityScale = 0;
            rb.gravityScale = 0;
            health.maxHealth = 10000;
            health.currentHealth = 10000;
            attack.baseDmg = 10;
        }
	}

    public static float Distance(Vector2 v)
    {
        if (instance == null)
            return float.MaxValue;
        return (v.ToV3() - instance.transform.position).magnitude;
    }

    public static void IgnoreEnemyCollisions(bool value)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), value);
    }

    public static Player instance;
    protected override void Update()
    {
        base.Update();
        dasher.Update();
        slamer.Update();
    }

    public override void AddBuff(Buff b)
    {
        base.AddBuff(b);
        b.ChangeToPlayerBuff();
    }

    public void AddPixel(int amount)
    {
        gold += amount;
        PickupsUI.Update();
    }

    public void AddOrbs(int amount)
    {
        orbs += amount;
        PickupsUI.Update();
    }

    class JumpInvoker : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.currentState.Jump();
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

    class BombDropInvoker : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            if (instance.orbs > 0)
            {
                OrbBomb.Drop(ap.unit.transform.position);
                instance.orbs--;
                PickupsUI.Update();
            }
            return false;
        }
    }
}
