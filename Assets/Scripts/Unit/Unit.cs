using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{
    public IController controller;
    public Rigidbody2D rb;
    public CircleCollider2D bot, top;
    public BoxCollider2D sides;
    public float speed;
    public EventManager eventManager;
    public Jumper jumper;
    public Mover mover;
    public UnitState currentState, walking, airborne;
    public Attack attack;
    public int direction = 1;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        eventManager = new EventManager(this);
        eventManager.SubscribeHandler("land", new WalkOnLand());
    }

    protected virtual void Update()
	{
        controller.Update();
        Vector2 needVel = controller.NeedVel();
        if (currentState == null)
            currentState = airborne;
        if (needVel.magnitude > 0)
        {
            currentState.Move(needVel);
        }
        currentState.Update();
        eventManager.Update();
        if (!bot.IsTouchingLayers(1 << LayerMask.NameToLayer("Level")))
        {
            currentState.Transit(airborne);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == tag)
        {
            return;
        }
        foreach (var a in collision.contacts)
        {
            if (a.otherCollider == bot)
            {
                eventManager.InvokeHandlers("land");
                break;
            }
        }
    }

    class WalkOnLand : EventHandler
    {
        public bool Handle(Unit u)
        {
            u.currentState.Transit(u.walking);
            return false;
        }
    }
}
