using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{
    public IController controller;
    public Rigidbody2D rb;
    public float speed;
    public EventManager eventManager;
    public Jumper jumper;
    public Mover mover;
    public UnitState state;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        eventManager = new EventManager(this);
    }

    protected virtual void Update()
	{
        controller.Update();
        state.Update();
        eventManager.Update();
    }
}
