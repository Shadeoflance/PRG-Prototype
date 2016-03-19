using UnityEngine;
using System.Collections;

public class Player : Unit
{
    public Dasher dasher;
    public Slamer slamer;
    public BoxCollider2D main;
	void Start()
	{
		controller = new PlayerController(this);
        jumper = new MultipleJumper(this, jumpForce, jumpHeight, 1);
        mover = new DefaultMover(this, speed);
        attack = new Weapon(this);
        health = new Health(this, 2);
        dasher = new DefaultDasher(this);
        slamer = new DefaultSlamer(this);
        currentState = new PlayerWalkingState(this);
        walking = new PlayerWalkingState(this);
        airborne = new PlayerAirborneState(this);
        eventManager.SubscribeHandler("jumpButtonDown", new JumpInvoker());
        eventManager.SubscribeHandler("dashButtonDown", new DashInvoker());
        if (instance == null)
            instance = this;
        else if (instance != this)
        {
            Destroy(instance.gameObject);
            instance = this;
        }
	}

    public static Player instance;

    protected override void Update()
    {
        base.Update();
        (attack as Weapon).Update();
        dasher.Update();
        slamer.Update();
    }

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (currentState is DashState)
    //    {
    //        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
    //        {
    //            ActionParams ap = new ActionParams();
    //            ap["enemy"] = collision.GetComponent<Enemy>();
    //            eventManager.InvokeHandlers("dashPenetrateEnemy", ap);
    //        }
    //        if (collision.gameObject.layer == LayerMask.NameToLayer("Level"))
    //        {
    //            ActionParams ap = new ActionParams();
    //            ap["level"] = collision.gameObject;
    //            eventManager.InvokeHandlers("dashPenetrateLevel", ap);
    //        }
    //    }
    //}

    class JumpInvoker : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.currentState.Jump();
            return false;
        }
    }
    //class AttackInvoker : ActionListener
    //{
    //    public bool Handle(ActionParams ap)
    //    {
    //        ap.unit.currentState.Attack();
    //        return false;
    //    }
    //}
    class DashInvoker : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            (ap.unit.currentState as PlayerState).Dash();
            return false;
        }
    }
}
