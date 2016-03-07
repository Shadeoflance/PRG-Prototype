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
    public Health health;
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
        if (!bot.IsTouchingLayers(1 << LayerMask.NameToLayer("Level")) && currentState != airborne)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 0.6f, 1 << LayerMask.NameToLayer("Level"));
            if(hit.collider == null)
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
            Debug.Log(name + " Collided with: " + a.collider.name);
            if (a.otherCollider == bot && currentState != walking)
            {
                eventManager.InvokeHandlers("land");
                break;
            }
        }
    }

    void OnGUI()
    {
        Vector2 pos = VectorUtils.V3ToV2(Camera.main.WorldToScreenPoint(transform.position));
        Rect rect = new Rect(0, 0, 150, 50);
        rect.x = pos.x - 30;
        rect.y = Screen.height - pos.y - rect.height;
        GUI.Label(rect, new GUIContent(currentState.GetType().ToString()));
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
