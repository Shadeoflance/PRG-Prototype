using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Player : Unit
{
    public Dasher dasher;
    public Slamer slamer;
    public BoxCollider2D main;
    public int gold = 0;
    Animator anim;
    int speedId;
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
        anim = sprite.GetComponent<Animator>();
        speedId = Animator.StringToHash("Speed");
    }
	void Start()
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
        anim.SetFloat(speedId, Mathf.Abs(rb.velocity.x));
    }

    public override void AddBuff(Buff b)
    {
        base.AddBuff(b);
        b.ChangeToPlayerBuff();
    }

    public void AddGold(int amount)
    {
        gold += amount;
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

    class DamageBoost : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.AddBuff(new Invulnerability(ap.unit, 1));
            return false;
        }
    }
}
