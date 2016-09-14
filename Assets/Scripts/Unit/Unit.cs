using UnityEngine;
using System.Collections;

public struct Stats
{
    public Stats(float hp, float speed, float damage)
    {
        this.speed = speed;
        this.damage = damage;
        this.hp = hp;
    }
    public float speed, damage, hp;
}

public class Unit : MonoBehaviour
{
    public float hp, damage, speed;

    public float gravityScale = 5;
    public IController controller;
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Collider2D bot, top;
    public Collider2D sides;
    public EventManager eventManager;
    public Jumper jumper;
    public Mover mover;
    public UnitState currentState, walking, airborne;
    public Attack attack;
    public Health health;
    public int direction = 1;
    protected Material healthBar;
    private Group<Buff> buffs = new Group<Buff>();
    public Vector2 size;
    public Stats stats;

    protected virtual void Awake()
    {
        stats = new Stats(hp, speed, damage);
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rb.gravityScale = gravityScale;
        eventManager = new EventManager(this);
        eventManager.SubscribeHandler("land", new WalkOnLand());
        size = sr.bounds.extents.ToV2();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
	{
        buffs.Refresh();
        foreach (var a in buffs)
            a.Update();
        controller.Update();
        if(attack != null)
            attack.Update();
        Vector2 needVel = controller.NeedVel();
        if (currentState == null)
            currentState = airborne;
        currentState.Move(needVel);
        currentState.Update();
        eventManager.Update();
        if (bot != null && !bot.IsTouchingLayers(1 << LayerMask.NameToLayer("Level")) && currentState != airborne)
        {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 0.6f, 1 << LayerMask.NameToLayer("Level"));
            //if(hit.collider == null)
            currentState.Transit(airborne);
        }
    }

    public virtual void AddBuff(Buff b)
    {
        buffs.Add(b);
    }

    public virtual void RemoveBuff(Buff b)
    {
        buffs.Remove(b);
    }

    protected virtual void FixedUpdate()
    {
        if (currentState == null)
            currentState = airborne;
        currentState.FixedUpdate();
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var a in collision.contacts)
        {
            if (a.otherCollider == bot && currentState != walking)
            {
                eventManager.InvokeHandlers("land", null);
                break;
            }
            if (a.otherCollider == sides && a.collider.gameObject.layer == LayerMask.NameToLayer("Level"))
            {
                eventManager.InvokeHandlers("levelSideHit", null);
            }
        }
    }

    //void OnGUI()
    //{
    //    Vector2 pos = Utils.ToV2(Camera.main.WorldToScreenPoint(transform.position));
    //    Rect rect = new Rect(0, 0, 150, 50);
    //    rect.x = pos.x - 30;
    //    rect.y = Screen.height - pos.y - rect.height;
    //    if(currentState == null)
    //        GUI.Label(rect, "null");
    //    else GUI.Label(rect, new GUIContent(currentState.GetType().ToString()));
    //}

    class WalkOnLand : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.currentState.Transit(ap.unit.walking);
            return false;
        }
    }
}
