using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour 
{
	protected IController controller;
	protected Rigidbody2D rb;
	public float Speed;

	protected void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		controller.Update();
		rb.velocity = VectorUtils.TrimX(rb.velocity);
		Vector2 needVel = controller.NeedVel();
		rb.velocity += VectorUtils.TrimY(needVel) * Speed;
		if(needVel.x > 0 && transform.localScale.x < 0)
		{
			transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
		}
		else if(needVel.x < 0 && transform.localScale.x > 0)
		{
			transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
		}
	}
}
