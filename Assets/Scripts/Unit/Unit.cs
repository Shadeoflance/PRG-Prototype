using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{
    public float jumpForce, jumpHeight, gravityScale = 5;
    public IController controller;
    public Rigidbody2D rb;
    public Collider2D bot, top;
    public Collider2D sides;
    public float speed;
    public EventManager eventManager;
    public Jumper jumper;
    public Mover mover;
    public UnitState currentState, walking, airborne;
    public Attack attack;
    public Health health;
    public int direction = 1;
    private int spriteDirection = 1;
    public int damage = 1, hp = 3;
    private Transform sprite;
    private Material healthBar;
    public Group<Buff> buffs = new Group<Buff>();

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        eventManager = new EventManager(this);
        eventManager.SubscribeHandler("land", new WalkOnLand());
        sprite = transform.FindChild("Sprite");
        healthBar = transform.FindChild("HealthBar").GetComponent<SpriteRenderer>().material;
    }

    protected virtual void Update()
	{
        buffs.Refresh();
        foreach (var a in buffs)
            a.Update();
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
        healthBar.SetFloat("_HealthPercentage", 1f * health.currentHealth / health.maxHealth);
        if (!bot.IsTouchingLayers(1 << LayerMask.NameToLayer("Level")) && currentState != airborne)
        {
            //RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(0, -1), 0.6f, 1 << LayerMask.NameToLayer("Level"));
            //if(hit.collider == null)
            currentState.Transit(airborne);
        }
        if (spriteDirection != direction)
        {
            sprite.localScale = Vector3.Scale(sprite.localScale, new Vector3(-1, 1, 1));
            spriteDirection *= -1;
        }
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

    void OnGUI()
    {
        Vector2 pos = VectorUtils.V3ToV2(Camera.main.WorldToScreenPoint(transform.position));
        Rect rect = new Rect(0, 0, 150, 50);
        rect.x = pos.x - 30;
        rect.y = Screen.height - pos.y - rect.height;
        GUI.Label(rect, new GUIContent(currentState.GetType().ToString()));
    }

    class WalkOnLand : ActionListener
    {
        public bool Handle(ActionParams ap)
        {
            ap.unit.currentState.Transit(ap.unit.walking);
            return false;
        }
    }
}
